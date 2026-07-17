using JobFlowProject.Business.Exceptions.Authentication_Exceptions;
using JobFlowProject.Business.Exceptions.BaseExeption;
using JobFlowProject.Business.Interfaces.User;
using JobFlowProject.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace JobFlowProject.Business.Services.User;

public class AdminService : IAdminService
{
    private readonly UserManager<AppUser> _userManager;

    public AdminService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task VerifyEmployerAsync(Guid employerId)
    {
        var employer = await _userManager.FindByIdAsync(employerId.ToString());

        if (employer is null)
            throw new UserNotFoundException();

        employer.IsApproved = true;

        var result = await _userManager.UpdateAsync(employer);

        if (!result.Succeeded)
            throw new Exception(result.Errors.FirstOrDefault()?.Description);
    }
}