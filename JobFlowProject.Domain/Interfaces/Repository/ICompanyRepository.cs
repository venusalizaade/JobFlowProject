using JobFlowProject.Domain.Entities.Componies;

namespace JobFlowProject.Domain.Interfaces.Repository;

public interface ICompanyRepository
{

    Task<Company?> GetByIdAsync(Guid companyId);
}