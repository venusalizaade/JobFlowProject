using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Enums;
using JobFlowProject.Domain.Interfaces.Repository.User;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace JobFlowProject.Infrastructure.Repositories.User;

public class AdminRepository : IAdminRepository
{
    private readonly JobFlowDbContext _context;

    public AdminRepository(JobFlowDbContext context)
    {
        _context = context;
    }

    public async Task<AppUser?> GetEmployerDetailsAsync(Guid employerId)
    {
        return await _context.Users
            .Include(x => x.Company)
            .FirstOrDefaultAsync(x =>
                x.Id == employerId &&
                x.CompanyId != null &&
                !x.IsDeleted);
    }

    public async Task<AppUser?> GetJobSeekerDetailsAsync(Guid jobSeekerId)
    {
        return await _context.Users
            .Include(x => x.Attachments)
            .FirstOrDefaultAsync(x =>
                x.Id == jobSeekerId &&
                x.CompanyId == null &&
                !x.IsDeleted);
    }

    public async Task<int> GetTotalCompaniesAsync()
    {
        return await _context.Companies.CountAsync(x => !x.IsDeleted);
    }

    public async Task<int> GetTotalJobPostsAsync()
    {
        return await _context.JobPosts.CountAsync(x => !x.IsDeleted);
    }

    public async Task<int> GetPendingApplicationsAsync()
    {
        return await _context.JobApplications
            .CountAsync(x => x.Status == JobApplicationStatusEnum.Pending);
    }

}