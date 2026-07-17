using JobFlowProject.Domain.Entities.Job;

namespace JobFlowProject.Domain.Interfaces.Repository;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();

    Task<Category?> GetByIdAsync(Guid id);
}