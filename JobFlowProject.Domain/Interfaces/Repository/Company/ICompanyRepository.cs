using JobFlowProject.Domain.Entities.Componies;

namespace JobFlowProject.Domain.Interfaces.Repository;

public interface ICompanyRepository
{

    Task<Company?> GetByCompanyIdAsync(Guid companyId);

    Task<Company?> GetByAppUserIdAsync(Guid appUserId);
}