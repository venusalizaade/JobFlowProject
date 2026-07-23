using JobFlowProject.Business.Constants;
using JobFlowProject.Business.Dto.CompanyDto;
using JobFlowProject.Business.Dto.User;
using JobFlowProject.Business.Exceptions.AuthenticationExceptions;
using JobFlowProject.Business.Exceptions.BaseExeption;
using JobFlowProject.Business.Interfaces.EmployerInterfaces;
using JobFlowProject.Domain.Entites.Resume;
using JobFlowProject.Domain.Entities.Componies;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Enums;
using JobFlowProject.Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Http;
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
            var company = await _repository.GetByIdAsync(companyId);


            if (company is null)

                throw new ItemNotFoundException("Company not found.");


            if (company.AppUserId != requesterId)

                throw new PermissionDeniedException();
        }

        public async Task UploadLogoAsync(Guid requesterId, IFormFile file)
        {
            var company = await _companyRepository.GetByAppUserIdAsync(requesterId);

            if (company is null)
                throw new ItemNotFoundException("Company not found.");

            if (file == null || file.Length == 0)
                throw new Exception("File is empty.");

            var uploadsFolder = Path.Combine("Uploads", "Companies");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var attachment = new AttachmentFile(
                fileName,
                filePath,
                file.ContentType,
                requesterId,
                AttachmentType.CompanyLogo);

            company.Attachments.Add(attachment);

            await _repository.UpdateAsync(company);
        }
    }
}