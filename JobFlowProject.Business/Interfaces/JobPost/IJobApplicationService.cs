using JobFlowProject.Business.Dto.Commands;
using JobFlowProject.Business.Dto.CompanyDto;
using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Business.Interfaces.JobPost;

public interface IJobApplicationService
{
    Task ApplyAsync(Guid requesterId, ApplyJobCommand command);


    Task<List<JobApplicationResponseDto>>
        GetJobApplicationsAsync(Guid requesterId, Guid jobPostId);


    Task ChangeStatusAsync(Guid requesterId, ChangeApplicationStatusCommand command);
}