using JobFlowProject.Business.Constants;
using JobFlowProject.Business.Dto.CompanyDto;
using JobFlowProject.Business.Dto.User;
using JobFlowProject.Business.Exceptions.AuthenticationExceptions;
using JobFlowProject.Business.Exceptions.BaseExeption;
using JobFlowProject.Business.Interfaces.EmployerInterfaces;
using JobFlowProject.Domain.Entities.Componies;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Identity;

namespace JobFlowProject.Business.Services.CompaneisService
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericRepository<Company> _repository;

        public CompanyService(
            ICompanyRepository companyRepository,
            UserManager<AppUser> userManager,
            IGenericRepository<Company> repository)
        {
            _companyRepository = companyRepository;
            _userManager = userManager;
            _repository = repository;
        }


        public async Task<CompanyResponseDto> GetCompanyInfoAsync(
            Guid companyId,
            Guid requesterId)

        {
            await EnsureUserIsApproved(requesterId);
            var company = await _companyRepository.GetByCompanyIdAsync(companyId);

            if (company is null)

                throw new ItemNotFoundException("Company not found.");

            if (company.AppUserId != requesterId)

                throw new PermissionDeniedException();


            return new CompanyResponseDto(
                company.Name,
                company.NationalId,
                company.CityId,
                company.ProvinceId,
                company.About
            );
        }


        public async Task UpdateByEmployerAsync(
            Guid companyId,
            Guid requesterId,
            UpdateCompanyRequestDto dto)

        {
            await EnsureUserIsApproved(requesterId);
            var company = await _repository.GetByIdAsync(companyId);


            if (company is null)

                throw new ItemNotFoundException("Company not found.");


            if (company.AppUserId != requesterId)

                throw new PermissionDeniedException();
        }

        private async Task EnsureUserIsApproved(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null || !user.IsApproved)
                throw new PermissionDeniedException("Account is not approved yet.");
        }
    }
}