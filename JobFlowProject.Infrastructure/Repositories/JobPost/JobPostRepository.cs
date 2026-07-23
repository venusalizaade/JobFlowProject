using JobFlowProject.Domain.Entities.Job;
using JobFlowProject.Domain.Enums;
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
    public async Task<List<JobPost>> GetActiveAsync()
    {
        return await _context.JobPosts
            .Where(x => x.IsActive)
            .Include(x => x.Company)
            .Include(x => x.Category)
            .Include(x => x.City)
            .Include(x => x.Skill)
            .ToListAsync();
    }
    
    public async Task<List<JobPost>> SearchAsync(
        string? title,
        Guid? categoryId,
        Guid? skillId,
        EmploymentTypeEnum? employmentType,
        decimal? minSalary,
        decimal? maxSalary,
        Guid? cityId,
        Guid? provinceId)
    {
        var query = _context.JobPosts
            .Include(x => x.Company)
            .Include(x => x.Category)
            .Include(x => x.City)
            .Include(x => x.Skill)
            .Where(x => x.IsActive)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(x => x.Title.Contains(title));

        if (categoryId.HasValue)
            query = query.Where(x => x.CategoryId == categoryId);

        if (skillId.HasValue)
            query = query.Where(x => x.SkillId == skillId);

        if (employmentType.HasValue)
            query = query.Where(x => x.EmploymentType == employmentType);

        if (cityId.HasValue)
            query = query.Where(x => x.CityId == cityId);

        if (provinceId.HasValue)
            query = query.Where(x => x.ProvinceId == provinceId);

        if (minSalary.HasValue)
            query = query.Where(x => x.Salary.HasValue && x.Salary.Value >= minSalary.Value);

        if (maxSalary.HasValue)
            query = query.Where(x => x.Salary.HasValue && x.Salary.Value <= maxSalary.Value);
        return await query.ToListAsync();
    }
    public async Task<List<JobPost>> SearchAsync(
        string? title,
        EmploymentTypeEnum? employmentType,
        Guid? cityId)
    {
        var query = _context.JobPosts
            .Where(x => x.IsActive);

        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(x => x.Title.Contains(title));

        if (employmentType.HasValue)
            query = query.Where(x => x.EmploymentType == employmentType.Value);

        if (cityId.HasValue)
            query = query.Where(x => x.CityId == cityId.Value);

        return await query.ToListAsync();
    }
    public async Task<List<JobPost>> FilterAsync(Guid? categoryId, Guid? skillId, decimal? minSalary, decimal? maxSalary)
    {
        var query = _context.JobPosts
            .Where(x => x.IsActive);

        if (categoryId.HasValue)
            query = query.Where(x => x.CategoryId == categoryId.Value);

        if (skillId.HasValue)
            query = query.Where(x => x.SkillId == skillId.Value);

        if (minSalary.HasValue)
            query = query.Where(x => x.Salary >= minSalary.Value);

        if (maxSalary.HasValue)
            query = query.Where(x => x.Salary <= maxSalary.Value);

        return await query.ToListAsync();
    }
}