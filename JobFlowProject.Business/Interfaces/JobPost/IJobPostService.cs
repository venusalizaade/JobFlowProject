using JobFlowProject.Business.Dto.Commands;
using JobFlowProject.Business.Dto.JobPost;
using JobFlowProject.Domain.Enums;
using WebApplication1.Dto.Authentication;
using JobPostResponseDto = JobFlowProject.Business.Dto.JobPost.JobPostResponseDto;
using JobPostSearchRequestDto = JobFlowProject.Business.Dto.JobPost.JobPostSearchRequestDto;

namespace JobFlowProject.Business.Interfaces.JobPost;

public interface IJobPostService
{
    Task<JobPostResponseDto> CreateAsync(Guid requesterId, CreateJobPostCommand command);

    Task<List<JobPostResponseDto>> GetCompanyJobPostsAsync(Guid requesterId);

    Task<JobPostDetailDto> GetDetailsAsync(Guid id);

    Task UpdateAsync(Guid requesterId, Guid jobPostId, UpdateJobPostCommand command);

    Task DeactivateAsync(Guid requesterId, Guid jobPostId);
    Task<List<JobPostResponseDto>> GetActiveAsync();
    
    Task<List<JobPostResponseDto>> SearchAsync(JobPostSearchRequestDto dto);
    
    Task<List<JobPostResponseDto>> FilterAsync(JobPostFilterRequestDto dto);
    }