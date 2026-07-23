using JobFlowProject.Business.Dto.Commands;
using JobFlowProject.Business.Dto.JobPost;
using JobPostResponseDto = JobFlowProject.Business.Dto.JobPost.JobPostResponseDto;

namespace JobFlowProject.Business.Interfaces.JobPost;

public interface IJobPostService
{
    Task<JobPostResponseDto> CreateAsync(Guid requesterId, CreateJobPostCommand command);

    Task<List<JobPostResponseDto>> GetCompanyJobPostsAsync(Guid requesterId);

    Task<JobPostDetailDto> GetDetailsAsync(Guid id);

    Task UpdateAsync(Guid requesterId, Guid jobPostId, UpdateJobPostCommand command);

    Task DeactivateAsync(Guid requesterId, Guid jobPostId);
    Task<List<JobPostResponseDto>> GetActiveAsync();
}