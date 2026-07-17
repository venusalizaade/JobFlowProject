using System.Linq.Expressions;
using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities;

namespace JobFlowProject.Domain.Interfaces.Repository;

public interface IGenericRepository <TEntity> where TEntity : BaseEntity
{
   Task AddAsync(TEntity entity);
   
   Task<List<TEntity>> QueryAsync(
      Expression<Func<TEntity, bool>> predicate,
      Paging paging,
      bool traking=false);

   Task<TEntity?> GetByIdAsync(Guid id, bool tracking=false);
   
   Task UpdateAsync(TEntity entity);
   
   Task HardDeleteAsync(Guid id);
   
   Task SoftDeleteAsync(Guid id, Guid requesterId);
   
 
   
}