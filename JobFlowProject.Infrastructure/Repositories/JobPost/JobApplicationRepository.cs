using JobFlowProject.Domain.Entities.Job;
using JobFlowProject.Domain.Enums;
using JobFlowProject.Domain.Interfaces.Repository;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace JobFlowProject.Infrastructure.Repositories;

public class JobApplicationRepository : GenericRepository<JobApplication>, IJobApplicationRepository
{
private readonly JobFlowDbContext _context;

    
public JobApplicationRepository(JobFlowDbContext context) : base(context)
{
    _context = context;
}

public async Task<bool> ExistsAsync(Guid jobPostId, Guid applicantId)
{
    return await _context.JobApplications
        .AnyAsync(x =>
            x.JobPostId == jobPostId &&
            x.JobSeekerId == applicantId &&
            !x.IsDeleted);
}

public async Task<bool> HasAppliedAsync(Guid jobPostId, Guid jobSeekerId)
{
    return await _context.JobApplications
        .AnyAsync(x =>
            x.JobPostId == jobPostId &&
            x.JobSeekerId == jobSeekerId &&
            !x.IsDeleted);
}

public async Task<List<JobApplication>> GetByJobPostAsync(Guid jobPostId)
{
    return await _context.JobApplications
        .Include(x => x.JobSeeker)
        .Where(x => x.JobPostId == jobPostId && !x.IsDeleted)
        .ToListAsync();
}

public async Task<List<JobApplication>> GetByJobSeekerIdAsync(Guid jobSeekerId)
{
    return await _context.JobApplications
        .Include(x => x.JobPost)
            .ThenInclude(x => x.Company)
        .Where(x => x.JobSeekerId == jobSeekerId && !x.IsDeleted)
        .OrderByDescending(x => x.CreatedAt)
        .ToListAsync();
}

public async Task<JobApplication?> GetDetailsAsync(Guid applicationId)
{
    return await _context.JobApplications
        .Include(x => x.JobSeeker)
        .Include(x => x.JobPost)
            .ThenInclude(x => x.Company)
        .FirstOrDefaultAsync(x =>
            x.Id == applicationId &&
            !x.IsDeleted);
}

public async Task<JobApplication?> GetPendingApplicationAsync(
    Guid applicationId,
    Guid jobSeekerId)
{
    return await _context.JobApplications
        .Include(x => x.JobPost)
            .ThenInclude(x => x.Company)
        .FirstOrDefaultAsync(x =>
            x.Id == applicationId &&
            x.JobSeekerId == jobSeekerId &&
            x.Status == JobApplicationStatusEnum.Pending &&
            !x.IsDeleted);
}
}