using JobFlowProject.Domain.Entities.Job;
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
            .AnyAsync(x => x.JobPostId == jobPostId && x.JobSeekerId == applicantId);
    }

    public async Task<List<JobApplication>> GetByJobPostAsync(Guid jobPostId)
    {
        return await _context.JobApplications
            .Include(x => x.JobSeeker)
            .Where(x => x.JobPostId == jobPostId)
            .ToListAsync();
    }
}