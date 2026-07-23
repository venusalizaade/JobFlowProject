using JobFlowProject.Domain.Entities;
using JobFlowProject.Domain.Interfaces.Repository.User;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace JobFlowProject.Infrastructure.Repositories.User;

public class RefreshTokenRepository
    : GenericRepository<RefreshToken>,
        IRefreshTokenRepository
{
    private readonly JobFlowDbContext _context;

    public RefreshTokenRepository(JobFlowDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .Include(x => x.AppUser)
            .FirstOrDefaultAsync(x => x.Token == token);
    }
}