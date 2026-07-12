using System.Linq.Expressions;
using JobFlowProject.Domain.Entites;
using JobFlowProject.Domain.Entites.User;

namespace JobFlowProject.Domain.Interfaces.Reposiyories;

public interface IGenericRepository <TEntity> where TEntity : BaseEntity
{
   Task AddAsync(TEntity entity);
   
   Task<List<TEntity>> QueryAsync(
      Expression<Func<TEntity, bool>> predicate,
      Paging paging,
      bool traking=false);

   Task<TEntity?> GetByIdAsync(Guid id, bool traking=false);
   
   Task UpdateAsync(TEntity entity);
   
   Task HardDeleteAsync(Guid id);
   
   Task SoftDeleteAsync(Guid id, Guid requesterId);
   
 
   
}