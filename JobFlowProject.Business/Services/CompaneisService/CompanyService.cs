using JobFlowProject.Business.Constants;
using JobFlowProject.Business.Dto.CompanyDto;
using JobFlowProject.Business.Dto.User;
using JobFlowProject.Business.Exceptions.AuthenticationExceptions;
using JobFlowProject.Business.Exceptions.BaseExeption;
using JobFlowProject.Business.Interfaces.EmployerInterfaces;
using JobFlowProject.Domain.Entities.Componies;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Interfaces.Repository;

namespace JobFlowProject.Business.Services.CompaneisService
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IGenericRepository<Company> _repository;

        public CompanyService(
            ICompanyRepository companyRepository,
            IGenericRepository<Company> repository)
        {
            _companyRepository = companyRepository;
            _repository = repository;
        }

        public async Task<CompanyResponseDto> GetCompanyInfoAsync(
            Guid companyId,
            Guid requesterId)
        {
            var company = await _companyRepository.GetByIdAsync(companyId);

            if (company is null)
                throw new ItemNotFoundException("Company not found.");

            if (company.AppUserId != requesterId)
                throw new PermissionDeniedException();

            return new CompanyResponseDto(
                company.Name,
                company.NationalId,
                company.City,
                company.Province,
                company.About
            );
        }

        public async Task UpdateByEmployerAsync(
            Guid companyId,
            Guid requesterId,
            UpdateCompanyRequestDto dto)
        {
            var company = await _repository.GetByIdAsync(companyId);

            if (company is null)
                throw new ItemNotFoundException("Company not found.");

            if (company.AppUserId != requesterId)
                throw new PermissionDeniedException();

            company.Name = dto.Name;
            company.City = dto.City;
            company.Province = dto.Province;
            company.About = dto.About;
            company.Address = dto.Address;

            await _repository.UpdateAsync(company);
        }
    }
    
}