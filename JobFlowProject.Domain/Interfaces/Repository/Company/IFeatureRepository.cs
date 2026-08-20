using JobFlowProject.Domain.Entities.Componies.ComponyFeatures;

namespace JobFlowProject.Domain.Interfaces.Repository;

public interface IFeatureRepository : IGenericRepository<Feature>
{
    Task<List<Feature>> GetAllAsync();

    Task<List<Feature>> GetActiveAsync();

    Task<Feature?> GetByNameAsync(string name);
}