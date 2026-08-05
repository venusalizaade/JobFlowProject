using JobFlowProject.Business.Dto.Commands;
using JobFlowProject.Business.Dto.CompanyDto;
using JobFlowProject.Business.Dto.JobPost;
using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Business.Interfaces.JobPost;

public interface IJobApplicationService
{
    Task ApplyAsync(Guid requesterId, ApplyJobCommand command);


    Task<List<JobApplicationDto>>
        GetJobApplicationsAsync(Guid requesterId, Guid jobPostId);


    Task ChangeStatusAsync(Guid requesterId, ChangeApplicationStatusCommand command);
    

    Task<List<JobApplicationResponseDto>> GetMyApplicationsAsync(Guid requesterId);

    Task<JobApplicationDetailDto> GetDetailsAsync(Guid requesterId, Guid applicationId);

    Task CancelAsync(Guid requesterId, Guid applicationId);
}