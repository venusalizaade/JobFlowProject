using JobFlowProject.Business.Dto.CompanyDto;
using JobFlowProject.Business.Dto.User;
using JobFlowProject.Domain.Entities.Componies;
using Microsoft.AspNetCore.Http;

namespace JobFlowProject.Business.Interfaces.EmployerInterfaces;

public interface ICompanyService
{
    Task<CompanyResponseDto> GetCompanyInfoAsync(Guid companyId, Guid requesterId);
    
    Task UpdateByEmployerAsync(Guid companyId, Guid requesterId, UpdateCompanyRequestDto dto);
    
    Task UploadLogoAsync(Guid requesterId, IFormFile file);
}

