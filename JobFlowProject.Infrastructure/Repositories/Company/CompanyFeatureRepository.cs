using JobFlowProject.Domain.Entities.Componies.ComponyFeatures;
using JobFlowProject.Domain.Interfaces.Repository;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace JobFlowProject.Infrastructure.Repositories;

public class CompanyFeatureRepository
    : GenericRepository<CompanyFeature>, ICompanyFeatureRepository
{
    private readonly JobFlowDbContext _context;

    public CompanyFeatureRepository(JobFlowDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<CompanyFeature?> GetAssignedFeatureAsync(Guid companyId, Guid featureId)
    {
        return await _context.CompanyFeatures
            .FirstOrDefaultAsync(x =>
                x.CompanyId == companyId &&
                x.FeatureId == featureId &&
                !x.IsDeleted);
    }

    public async Task<List<CompanyFeature>> GetCompanyFeaturesAsync(Guid companyId)
    {
        return await _context.CompanyFeatures
            .Where(x => x.CompanyId == companyId && !x.IsDeleted && x.IsActive && x.EndDate > DateTime.UtcNow)
            .Include(x => x.Feature)
            .ToListAsync();
    }

    public async Task<List<CompanyFeature>> GetAllCompanyFeaturesAsync(Guid companyId)
    {
        return await _context.CompanyFeatures
            .Where(x => x.CompanyId == companyId && !x.IsDeleted)
            .Include(x => x.Feature)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }
}