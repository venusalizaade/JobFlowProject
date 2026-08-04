using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using JobFlowProject.Domain.Entities.Componies;
using JobFlowProject.Business.Constants;
using JobFlowProject.Business.Dto.Commands;
using JobFlowProject.Business.Dto.Token;
using JobFlowProject.Business.Exceptions.Authentication_Exceptions;
using JobFlowProject.Business.Exceptions.AuthenticationExceptions;
using JobFlowProject.Business.Interfaces;
using JobFlowProject.Business.Interfaces.Log;
using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Interfaces.Repository;
using JobFlowProject.Domain.Interfaces.Repository.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;


namespace JobFlowProject.Business.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IGenericRepository<Company> _companyRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly JwtSettings _jwtSettings;
    private readonly INotificationService _notificationService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    

    public AuthenticationService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        RoleManager<Role> roleManager,
        IOptions<JwtSettings> options, IGenericRepository<Company> companyRepository,
      INotificationService notificationService,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _jwtSettings = options.Value;
        _companyRepository = companyRepository;
        _notificationService = notificationService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<TokenLoginResult> LoginAsync(LoginCommand loginCommand)
    {
        var result = await _signInManager.PasswordSignInAsync
            (loginCommand.Username, loginCommand.Password, false, false);
       
        if (result.IsLockedOut)
            throw new AuthenticationException("User is locked out. Please try again 15 minutes later.");
       
        if (result.IsNotAllowed)
            throw new PermissionDeniedException();
        

        if (!result.Succeeded)
            throw new AuthenticationException("Invalid username or password.");

        var user = await _userManager.FindByNameAsync(loginCommand.Username);

        if (user == null)
        {
            throw new UserNotFoundException();
        }

        return await GenerateTokenAsync(user);
    }


private async Task<TokenLoginResult> GenerateTokenAsync(AppUser user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
        };
        
        var userRoles = (await _userManager.GetRolesAsync(user))
            .Select(r => new Claim(ClaimTypes.Role, r)).ToList();

        foreach (var claim in userRoles)
        {
            var role = _roleManager.Roles.FirstOrDefault(r => r.Name == claim.Value);
            
            if (role is null) continue;
            
            var roleClaims = await _roleManager.GetClaimsAsync(role);
            claims.AddRange(roleClaims);
        } 
        
        claims.AddRange(userRoles);
        
        var userClaims = await _userManager.GetClaimsAsync(user);

        claims.AddRange(userClaims);
       
        claims.Add(new Claim(
            "IsApproved",
            user.IsApproved.ToString().ToLower()));
       
        if (await _userManager.IsInRoleAsync(user, RoleConstants.AdminRoleName))
        {
            claims.Add(new Claim(
                ClaimConstants.CanApproveEmployer,
                "true"));
        }
        var refreshToken = Guid.NewGuid().ToString("N");

        var refreshTokenEntity = new RefreshToken(
            user.Id,
            refreshToken,
            DateTime.UtcNow.AddDays(30));

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);
        
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
       
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
       
        var expiresIn = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes);
        
        var token = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: expiresIn,
            signingCredentials: credentials);
        
        var accessToken = new JwtSecurityTokenHandler().WriteToken(token)!;
        var expiresInSeconds = expiresIn.Subtract(DateTime.UtcNow).TotalSeconds;
        return new TokenLoginResult(
            accessToken,
            refreshToken,
            expiresInSeconds);
    }


    public async Task<JobSeekerRegisterResult> JobSeekerRegisterAsync(RegisterJobSeekerCommand command)
    {
        var duplicateUser = await _userManager.FindByNameAsync(command.NationalId);

        if (duplicateUser != null)
            throw new DuplicateNationalIdException();

        var duplicateEmail = await _userManager.FindByEmailAsync(command.Email);

        if (duplicateEmail != null)
            throw new DuplicateEmailException();

        var user = new AppUser(
            command.FirstName,
            command.LastName,
            command.NationalId,
            command.Email,
            command.PhoneNumber,
            command.Gender);
        

        var result = await _userManager.CreateAsync(user, command.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"{error.Code} : {error.Description}");
            }

            throw new UserRegistrationException(
                string.Join(" | ", result.Errors.Select(x => $"{x.Code}: {x.Description}")));
        }

        var roleResult = await _userManager.AddToRoleAsync(user, RoleConstants.JobSeekerRoleName);

        if (!roleResult.Succeeded)
        {
            throw new RoleAssignmentException(
                roleResult.Errors.FirstOrDefault()?.Description ??
                "Failed to assign role.");
        }

        return new JobSeekerRegisterResult(user.Id);
    }


    public async Task<EmployerRegisterResult> EmployerRegisterAsync(RegisterEmployerCommand command)
    {
       
        if (await _userManager.FindByNameAsync(command.NationalId) != null)
            throw new DuplicateNationalIdException();

        if (await _userManager.FindByEmailAsync(command.Email) != null)
            throw new DuplicateEmailException();

       
        using var transaction =
            new System.Transactions.TransactionScope(System.Transactions.TransactionScopeAsyncFlowOption.Enabled);

        try
        {
           
            var user = new AppUser(
                command.FirstName,
                command.LastName,
                command.NationalId,
                command.Email,
                command.PhoneNumber,
                command.Gender);

            var userResult = await _userManager.CreateAsync(user, command.Password);
           
            if (!userResult.Succeeded)
                throw new UserRegistrationException(userResult.Errors.FirstOrDefault()?.Description ??
                                                    "Registration failed.");

          
            var company = new Company(
                command.CompanyName,
                command.CompanyNationalId,
                user.Id, 
                command.CityId,
                command.ProvinceId,
                command.Address);

            await _companyRepository.AddAsync(company);

           
            user.SetCompany(company.Id);
            await _userManager.UpdateAsync(user);

           
            var roleResult = await _userManager.AddToRoleAsync(user, RoleConstants.EmployerRoleName);
            if (!roleResult.Succeeded)
                throw new RoleAssignmentException(roleResult.Errors.FirstOrDefault()?.Description ??
                                                  "Failed to assign role.");

            
            await _notificationService.NotifyAdminForEmployerVerificationAsync(company);

         
            transaction.Complete();

            return new EmployerRegisterResult(user.Id, company.Id);
        }
        
        catch (Exception)
        {
           
            throw;
        }
    
}
    public async Task<TokenLoginResult> RefreshTokenAsync(string refreshToken)
    {
        var token =
            await _refreshTokenRepository.GetByTokenAsync(refreshToken);

        if (token is null)
            throw new AuthenticationException("Invalid refresh token.");

        if (token.IsRevoked)
            throw new AuthenticationException("Refresh token revoked.");

        if (token.ExpiresAt <= DateTime.UtcNow)
            throw new AuthenticationException("Refresh token expired.");
       
        token.Revoke();                                         
       
        await _refreshTokenRepository.UpdateAsync(token);
       
        return await GenerateTokenAsync(token.AppUser);
    }
    public async Task LogoutAsync(string refreshToken)
    {
        var token =
            await _refreshTokenRepository.GetByTokenAsync(refreshToken);

        if (token is null)
            throw new AuthenticationException("Invalid refresh token.");

        token.Revoke();

        await _refreshTokenRepository.UpdateAsync(token);
    }
}