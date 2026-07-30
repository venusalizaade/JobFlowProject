using JobFlowProject.Business.Dto.User;
using JobFlowProject.Domain.Entites.Resume;
using Microsoft.AspNetCore.Http;

namespace JobFlowProject.Business.Interfaces.User;

public interface IJobSeekerService
{
    Task <JobSeekerDetailsDto> GetProfileAsync(Guid requesterId);

    Task UpdateProfileAsync(Guid requesterId, UpdateJobSeekerProfileDto dto);
   
    Task UploadResumeAsync(Guid requesterId, IFormFile file);

    Task ReplaceResumeAsync(Guid requesterId, IFormFile file);

    Task DeleteResumeAsync(Guid requesterId);
    
    Task<AttachmentFile> GetResumeAsync(Guid requesterId);
}