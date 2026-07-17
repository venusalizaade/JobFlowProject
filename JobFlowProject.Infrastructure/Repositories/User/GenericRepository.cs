using System.Linq.Expressions;
using JobFlowProject.Domain.Entites;
using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities;
using JobFlowProject.Domain.Interfaces.Repository;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace JobFlowProject.Infrastructure.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly JobFlowDbContext DbContext;

    public GenericRepository(JobFlowDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task AddAsync(TEntity entity)
    {
        await DbContext.Set<TEntity>().AddAsync(entity);
        await DbContext.SaveChangesAsync();
    }

    public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> predicate, Paging paging,
        bool traking = false)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();

        if (!traking) query = query.AsNoTracking();

        query = query.Where(predicate)
            .OrderByDescending(x => x.CreatedAt);

        return await query
            .Skip(paging.Skip)
            .Take(paging.PageSize)
            .ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, bool traking = false)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();
        
            
        if (!traking) 
            query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(x =>
            x.Id == id &&
            !x.IsDeleted);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);
        await DbContext.SaveChangesAsync();
    }

    public async Task HardDeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity is null) return;
        DbContext.Set<TEntity>().Remove(entity);
        await DbContext.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(Guid id, Guid requesterId)
    {
        var entity = await GetByIdAsync(id);
        if (entity is null) return;
        entity.SetAsDeleted(requesterId);
        await DbContext.SaveChangesAsync();
    }
    
    
}