using JobFlowProject.Business.Constants;
using JobFlowProject.Business.Dto.Admin;
using JobFlowProject.Business.Dto.JobPost;
using JobFlowProject.Business.Exceptions.Authentication_Exceptions;
using JobFlowProject.Business.Exceptions.BaseExeption;
using JobFlowProject.Business.Interfaces.User;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Enums;
using JobFlowProject.Domain.Interfaces.Repository;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobFlowProject.Business.Services.User;

public class AdminService : IAdminService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly JobFlowDbContext _context;
    private readonly IJobPostRepository _jobPostRepository;

    public AdminService(
        UserManager<AppUser> userManager,
        JobFlowDbContext context,
        IJobPostRepository jobPostRepository)
    {
        _userManager = userManager;
        _context = context;
        _jobPostRepository = jobPostRepository;
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
    public async Task<DashboardDto> GetDashboardAsync()
    {
        var totalUsers = await _userManager.Users.CountAsync();

        var totalEmployers = await _userManager.GetUsersInRoleAsync(RoleConstants.EmployerRoleName);

        var totalJobSeekers = await _userManager.GetUsersInRoleAsync(RoleConstants.JobSeekerRoleName);

        var totalCompanies = await _context.Companies.CountAsync();

        var totalJobPosts = await _context.JobPosts.CountAsync();

        var employers = await _userManager.GetUsersInRoleAsync(RoleConstants.EmployerRoleName);

        var pendingEmployers = employers.Count(x => !x.IsApproved);

        var pendingApplications = await _context.JobApplications
            .CountAsync(x => x.Status == JobApplicationStatusEnum.Pending);

        return new DashboardDto(
            totalUsers,
            totalEmployers.Count,
            totalJobSeekers.Count,
            totalCompanies,
            totalJobPosts,
            pendingEmployers,
            pendingApplications);
    }
    public async Task<List<EmployerListDto>> GetEmployersAsync()
    {
        var employers = await _userManager.Users
            .Where(x => x.CompanyId != null)
          
            .Select(x => new EmployerListDto(
                x.Id,
                $"{x.FirstName} {x.LastName}",
                x.Email!,
                x.PhoneNumber!,
                x.Company != null ? x.Company.Name : null,
                x.IsApproved
            ))
            .ToListAsync();

        return employers;
    }
    
    public async Task RejectEmployerAsync(Guid employerId)
    {
        var employer = await _userManager.FindByIdAsync(employerId.ToString());

        if (employer is null)
            throw new UserNotFoundException();

        employer.IsApproved = false;

        var result = await _userManager.UpdateAsync(employer);

        if (!result.Succeeded)
            throw new Exception(result.Errors.First().Description);
    }
    public async Task<List<JobPostListDto>> GetJobPostsAsync()
    {
        var jobs = await _jobPostRepository.GetAllAsync();

        return jobs.Select(x => new JobPostListDto(
            x.Id,
            x.Title,
            x.Company.Name,
            x.Category.Name,
            x.City.Name,
            x.Salary,
            x.IsActive
        )).ToList();
    }

    public async Task DeleteJobPostAsync(Guid id, Guid requesterId)
    {
        var job = await _jobPostRepository.GetByIdAsync(id);

        if (job is null)
            throw new ItemNotFoundException();

        await _jobPostRepository.SoftDeleteAsync(id, requesterId);
    }
    
    public async Task<List<JobSeekerListDto>> GetJobSeekersAsync()
    {
        var seekers = await _userManager.Users
            .Where(x => x.CompanyId == null)
            .Select(x => new JobSeekerListDto(
                x.Id,
                $"{x.FirstName} {x.LastName}",
                x.Email!,
                x.PhoneNumber!,
                x.Gender,
                x.About
            ))
            .ToListAsync();

        return seekers;
    }
    public async Task DeleteJobSeekerAsync(Guid id, Guid requesterId)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user is null)
            throw new UserNotFoundException();

        user.SetAsDeleted(requesterId);

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            throw new Exception(result.Errors.First().Description);
    }
}