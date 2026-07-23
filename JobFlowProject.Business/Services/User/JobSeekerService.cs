using JobFlowProject.Business.Dto.User;
using JobFlowProject.Business.Exceptions.Authentication_Exceptions;
using JobFlowProject.Business.Exceptions.BaseExeption;
using JobFlowProject.Business.Interfaces.User;
using JobFlowProject.Domain.Entites.Resume;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Infrastructure.Repositories;
using JobFlowProject.Infrastructure.Repositories.User;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace JobFlowProject.Business.Services.User;

public class JobSeekerService : IJobSeekerService

{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAttachmentRepository _attachmentRepository;
    private readonly IWebHostEnvironment _environment;

    public JobSeekerService(UserManager<AppUser> userManager, IAttachmentRepository attachmentRepository,
        IWebHostEnvironment environment)
    {
        _userManager = userManager;
        _attachmentRepository = attachmentRepository;
        _environment = environment;
       
    }

    public async Task<JobSeekerProfileDto> GetProfileAsync(Guid requesterId)
    {
        var user = await _userManager.FindByIdAsync(requesterId.ToString());

        if (user is null)
            throw new UserNotFoundException();

        return new JobSeekerProfileDto(
            user.FirstName,
            user.LastName,
            user.Email!,
            user.PhoneNumber!,
            user.Gender,
            user.About
        );
    }

    public async Task UpdateProfileAsync(Guid requesterId, UpdateJobSeekerProfileDto dto)
    {
        var user = await _userManager.FindByIdAsync(requesterId.ToString());
        

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.PhoneNumber = dto.PhoneNumber;
        user.Gender=dto.Gender;
        user.About=dto.About;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            throw new Exception(
                result.Errors.FirstOrDefault()?.Description ??
                "Profile update failed.");
    }
    
    public async Task UploadResumeAsync(Guid requesterId, IFormFile file)
    {
        var user = await _userManager.FindByIdAsync(requesterId.ToString());

        if (user is null)
            throw new UserNotFoundException();

        if (file == null || file.Length == 0)
            throw new Exception("File is required.");

        if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
            throw new Exception("Only PDF files are allowed.");

        var folder = Path.Combine(_environment.WebRootPath, "Resumes");

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        var fileName = Guid.NewGuid() + ".pdf";

        var path = Path.Combine(folder, fileName);

        using var stream = new FileStream(path, FileMode.Create);

        await file.CopyToAsync(stream);
        
        var oldResume = await _attachmentRepository.GetByUserIdAsync(requesterId);

        if (oldResume != null)
            throw new Exception("Resume already exists.");

        var attachment = new AttachmentFile(
            file.FileName,
            path,
            file.ContentType,
            requesterId);

        attachment.Validate();

        await _attachmentRepository.AddAsync(attachment);
    }
    public async Task ReplaceResumeAsync(Guid requesterId, IFormFile file)
    { 
        var user = await _userManager.FindByIdAsync(requesterId.ToString());

        if (user is null)
            throw new UserNotFoundException();
        
        var attachment = await _attachmentRepository.GetByUserIdAsync(requesterId);

        if (attachment is null)
            throw new ItemNotFoundException("Resume not found.");

        if (File.Exists(attachment.FilePath))
            File.Delete(attachment.FilePath);

        var folder = Path.Combine(_environment.WebRootPath, "Resumes");
        
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        var fileName = Guid.NewGuid() + ".pdf";

        var path = Path.Combine(folder, fileName);

        using var stream = new FileStream(path, FileMode.Create);

        await file.CopyToAsync(stream);

        attachment.SetFile(
            file.FileName,
            path,
            file.ContentType);

        attachment.Validate();

        await _attachmentRepository.UpdateAsync(attachment);
    }
    
    public async Task DeleteResumeAsync(Guid requesterId)
    {
        var user = await _userManager.FindByIdAsync(requesterId.ToString());

        if (user is null)
            throw new UserNotFoundException();
        
        var attachment =
            await _attachmentRepository.GetByUserIdAsync(requesterId);
        
        if (attachment is null)
            throw new ItemNotFoundException("Resume not found.");

        if (File.Exists(attachment.FilePath))
            File.Delete(attachment.FilePath);

        await _attachmentRepository.SoftDeleteAsync(attachment.Id, requesterId);
    }
    public async Task<AttachmentFile> GetResumeAsync(Guid requesterId)
    {
        var attachment = await _attachmentRepository.GetByUserIdAsync(requesterId);

        if (attachment is null)
            throw new ItemNotFoundException("Resume not found.");

        return attachment;
    }
}