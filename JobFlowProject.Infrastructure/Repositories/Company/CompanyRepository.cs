using JobFlowProject.Domain.Entities.Componies;
using JobFlowProject.Domain.Interfaces.Repository;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace JobFlowProject.Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly JobFlowDbContext _context;


    public CompanyRepository(JobFlowDbContext context)

    {
        _context = context;
    }


    public async Task<Company?> GetByCompanyIdAsync(Guid companyId)

    {
        return await _context.Companies
            .FirstOrDefaultAsync(c => c.Id == companyId);
    }

    public async Task<Company?> GetByAppUserIdAsync(Guid appUserId)

    {
        return await _context.Companies
            .FirstOrDefaultAsync(c => c.AppUserId == appUserId);
    }
}