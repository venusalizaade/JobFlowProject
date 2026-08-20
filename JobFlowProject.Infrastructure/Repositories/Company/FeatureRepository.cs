using JobFlowProject.Domain.Entities.Componies.ComponyFeatures;
using JobFlowProject.Domain.Interfaces.Repository;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace JobFlowProject.Infrastructure.Repositories;

public class FeatureRepository
    : GenericRepository<Feature>, IFeatureRepository
{
    private readonly JobFlowDbContext _context;

    public FeatureRepository(JobFlowDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<List<Feature>> GetAllAsync()
    {
        return await _context.Features
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<List<Feature>> GetActiveAsync()
    {
        return await _context.Features
            .AsNoTracking()
            .Where(x => x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<Feature?> GetByNameAsync(string name)
    {
        return await _context.Features
            .FirstOrDefaultAsync(x => x.Name == name);
    }
}