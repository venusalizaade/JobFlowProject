using JobFlowProject.Business.Dto.ComponyDto;
using JobFlowProject.Domain.Entites.Componyes;

namespace JobFlowProject.Business.Interfaces.EmployerInterfaces;

public interface IComponyService
{
    Task <ComponyResponseDto> ComponyInfoAsync(Guid companyId);
    Task UpdateAsync(Company company);
}

