using System.Data;
using System.Linq.Expressions;
using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Interfaces.Repository;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JobFlowProject.Infrastructure.Repositories;

public class AppUserRepository : IUserRepository
{
    private readonly JobFlowDbContext _dbContext;

    public AppUserRepository(JobFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AppUser?> GetUserByNationalIdAsync(string nationalId)
    {

        return await _dbContext.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.NationalId == nationalId);
        
    }


}
   