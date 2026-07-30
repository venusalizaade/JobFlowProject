using JobFlowProject.Domain.Entities.Componies.ComponyFeatures;
using JobFlowProject.Domain.Interfaces.Repository;

namespace JobFlowProject.Infrastructure.Repositories;

public interface ICompanyFeatureRepository : IGenericRepository<CompanyFeature>
{
    Task<CompanyFeature?> GetAssignedFeatureAsync(Guid companyId, Guid featureId);

    Task<List<CompanyFeature>> GetCompanyFeaturesAsync(Guid companyId);
}
    
