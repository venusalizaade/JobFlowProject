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
        .Where(x => x.CompanyId == companyId && !x.IsDeleted)
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
        .Include(x => x.Skill)
        .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
}

public async Task<List<JobPost>> GetActiveAsync()
{
    var now = DateTime.UtcNow;

    return await _context.JobPosts
        .Include(x => x.Company)
        .Include(x => x.Category)
        .Include(x => x.City)
            .ThenInclude(c => c.Province)
        .Include(x => x.Skill)
        .Where(x =>
            x.IsActive &&
            !x.IsDeleted &&
            x.ExpiresAt > now)
        .OrderByDescending(x =>
            x.IsFeatured &&
            x.FeaturedUntil.HasValue &&
            x.FeaturedUntil > now)
        .ThenByDescending(x => x.CreatedAt)
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
    var now = DateTime.UtcNow;

    var query = _context.JobPosts
        .Include(x => x.Company)
        .Include(x => x.Category)
        .Include(x => x.City)
            .ThenInclude(c => c.Province)
        .Include(x => x.Skill)
        .Where(x =>
            x.IsActive &&
            !x.IsDeleted &&
            x.ExpiresAt > now)
        .AsQueryable();

    if (!string.IsNullOrWhiteSpace(title))
        query = query.Where(x => x.Title.Contains(title));

    if (categoryId.HasValue)
        query = query.Where(x => x.CategoryId == categoryId.Value);

    if (skillId.HasValue)
        query = query.Where(x => x.SkillId == skillId.Value);

    if (employmentType.HasValue)
        query = query.Where(x => x.EmploymentType == employmentType.Value);

    if (cityId.HasValue)
        query = query.Where(x => x.CityId == cityId.Value);

    if (provinceId.HasValue)
        query = query.Where(x => x.ProvinceId == provinceId.Value);

    if (minSalary.HasValue)
        query = query.Where(x => x.Salary.HasValue && x.Salary.Value >= minSalary.Value);

    if (maxSalary.HasValue)
        query = query.Where(x => x.Salary.HasValue && x.Salary.Value <= maxSalary.Value);

    return await query
        .OrderByDescending(x =>
            x.IsFeatured &&
            x.FeaturedUntil.HasValue &&
            x.FeaturedUntil > now)
        .ThenByDescending(x => x.CreatedAt)
        .ToListAsync();
}

public async Task<List<JobPost>> SearchAsync(
    string? title,
    EmploymentTypeEnum? employmentType,
    Guid? cityId)
{
    var now = DateTime.UtcNow;

    var query = _context.JobPosts
        .Include(x => x.Company)
        .Include(x => x.Category)
        .Include(x => x.City)
            .ThenInclude(c => c.Province)
        .Include(x => x.Skill)
        .Where(x =>
            x.IsActive &&
            !x.IsDeleted &&
            x.ExpiresAt > now)
        .AsQueryable();

    if (!string.IsNullOrWhiteSpace(title))
        query = query.Where(x => x.Title.Contains(title));

    if (employmentType.HasValue)
        query = query.Where(x => x.EmploymentType == employmentType.Value);

    if (cityId.HasValue)
        query = query.Where(x => x.CityId == cityId.Value);

    return await query
        .OrderByDescending(x =>
            x.IsFeatured &&
            x.FeaturedUntil.HasValue &&
            x.FeaturedUntil > now)
        .ThenByDescending(x => x.CreatedAt)
        .ToListAsync();
}

public async Task<List<JobPost>> FilterAsync(
    Guid? categoryId,
    Guid? skillId,
    decimal? minSalary,
    decimal? maxSalary)
{
    var now = DateTime.UtcNow;

    var query = _context.JobPosts
        .Include(x => x.Company)
        .Include(x => x.Category)
        .Include(x => x.City)
            .ThenInclude(c => c.Province)
        .Include(x => x.Skill)
        .Where(x =>
            x.IsActive &&
            !x.IsDeleted &&
            x.ExpiresAt > now)
        .AsQueryable();

    if (categoryId.HasValue)
        query = query.Where(x => x.CategoryId == categoryId.Value);

    if (skillId.HasValue)
        query = query.Where(x => x.SkillId == skillId.Value);

    if (minSalary.HasValue)
        query = query.Where(x => x.Salary.HasValue && x.Salary.Value >= minSalary.Value);

    if (maxSalary.HasValue)
        query = query.Where(x => x.Salary.HasValue && x.Salary.Value <= maxSalary.Value);

    return await query
        .OrderByDescending(x =>
            x.IsFeatured &&
            x.FeaturedUntil.HasValue &&
            x.FeaturedUntil > now)
        .ThenByDescending(x => x.CreatedAt)
        .ToListAsync();
}

public async Task<List<JobPost>> GetAllAsync()
{
    return await _context.JobPosts
        .Include(x => x.Company)
        .Include(x => x.Category)
        .Include(x => x.City)
            .ThenInclude(c => c.Province)
        .Include(x => x.Skill)
        .Where(x => !x.IsDeleted)
        .OrderByDescending(x => x.CreatedAt)
        .ToListAsync();
}

public async Task ToggleActiveAsync(Guid jobPostId, Guid requesterId)
{
    var job = await _context.JobPosts
        .FirstOrDefaultAsync(x => x.Id == jobPostId && !x.IsDeleted);

    if (job is null)
        throw new Exception("Job post not found.");

    job.ToggleStatus(requesterId);

    await _context.SaveChangesAsync();
}

public async Task SetFeaturedAsync(Guid jobPostId, int durationDays, Guid requesterId)
{
    var job = await _context.JobPosts
        .FirstOrDefaultAsync(x => x.Id == jobPostId && !x.IsDeleted);

    if (job is null)
        throw new Exception("Job post not found.");

    job.SetFeatured(durationDays, requesterId);

    await _context.SaveChangesAsync();
}

public async Task RemoveFeaturedAsync(Guid jobPostId, Guid requesterId)
{
    var job = await _context.JobPosts
        .FirstOrDefaultAsync(x => x.Id == jobPostId && !x.IsDeleted);

    if (job is null)
        throw new Exception("Job post not found.");

    job.RemoveFeatured(requesterId);

    await _context.SaveChangesAsync();
}


}
