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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace JobFlowProject.Business.Services.CompaneisService;
public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IGenericRepository<Company> _repository;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CompanyService(
        ICompanyRepository companyRepository,
        UserManager<AppUser> userManager,
        IGenericRepository<Company> repository,
        IWebHostEnvironment webHostEnvironment)
    {
        _companyRepository = companyRepository;
        _repository = repository;
        _webHostEnvironment = webHostEnvironment;
    }
    public async Task<CompanyResponseDto> GetCompanyInfoAsync(Guid companyId, Guid requesterId)
    {
       var company=await _companyRepository.GetByCompanyIdAsync(companyId);
       
       if (company is null)

           throw new ItemNotFoundException("Company not found.");

       if (company.AppUserId != requesterId)

           throw new PermissionDeniedException();

       return new CompanyResponseDto
       (
           company.Id,
           company.Name,
           company.NationalId,
           company.CityId,
           company.ProvinceId,
           company.About
       );

    }

    public async Task UpdateByEmployerAsync(Guid companyId, Guid requesterId, UpdateCompanyRequestDto dto)
    {
        var company =await _companyRepository.GetByCompanyIdAsync(companyId);
       
        if (company is null)
            throw new ItemNotFoundException("Company not found.");

        if (company.AppUserId != requesterId)
            throw new PermissionDeniedException();
        
        company.Name = dto.Name;
        company.CityId=dto.CityId;
        company.ProvinceId=dto.ProvinceId;
        company.Address=dto.Address;
        company.About = dto.About;
        
        await _repository.UpdateAsync(company);


    }

    public async Task UploadLogoAsync(Guid requesterId, IFormFile file)
    {
        var company = await _companyRepository.GetByAppUserIdAsync(requesterId);

        if (company is null)
            throw new ItemNotFoundException("Company not found.");

        if (file == null || file.Length == 0)
            throw new ItemNotFoundException("File is empty.");
        
        if (file.ContentType != "image/jpeg"|| file.ContentType != "pdf")
            throw new PermissionDeniedException("File is invalid.");
        
      
        var extention = Path.GetExtension(file.FileName);
       
        var newFileName = Guid.NewGuid() + extention;
        
        var uploadsFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "images");
        
        if(!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);
        
        var filePath = Path.Combine(uploadsFolder, newFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        var attachment = new AttachmentFile(
            newFileName,
            filePath,
            file.ContentType,
            requesterId,
            AttachmentType.CompanyLogo);

        company.Attachments.Add(attachment);

        await _repository.UpdateAsync(company);
    }
}
