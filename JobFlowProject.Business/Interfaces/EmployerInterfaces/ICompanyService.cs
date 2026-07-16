using JobFlowProject.Business.Dto.CompanyDto;
using JobFlowProject.Business.Dto.ComponyDto;
using JobFlowProject.Business.Dto.User;
using JobFlowProject.Domain.Entities.Componies;

namespace JobFlowProject.Business.Interfaces.EmployerInterfaces;

public interface ICompanyService
{
    Task<CompanyResponseDto> GetCompanyInfoAsync(
        Guid companyId,
        Guid requesterId);

    Task UpdateByEmployerAsync(
        Guid companyId,
        Guid requesterId,
        UpdateCompanyRequestDto dto);
}

