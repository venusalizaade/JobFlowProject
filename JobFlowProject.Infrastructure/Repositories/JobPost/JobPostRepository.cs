using JobFlowProject.Domain.Entities.Job;
using JobFlowProject.Domain.Interfaces.Repository;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace JobFlowProject.Infrastructure.Repositories;

public class JobPostRepository : GenericRepository<JobPost>, IJobPostRepository
{
    private readonly JobFlowDbContext _context;

    public JobPostRepository(JobFlowDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<List<JobPost>> GetCompanyJobPostsAsync(Guid companyId)
    {
        return await _context.JobPosts
            .AsNoTracking()
            .Where(x => x.CompanyId == companyId)
            .ToListAsync();
    }

    public async Task<JobPost?> GetJobPostDetailsAsync(Guid id)
    {
        return await _context.JobPosts
            .AsNoTracking()
            .Include(x => x.Company)
            .Include(x => x.Category)
            .Include(x => x.City)
            .ThenInclude(c => c.Province)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

   
}