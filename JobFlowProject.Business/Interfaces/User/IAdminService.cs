using JobFlowProject.Business.Dto.Admin;
using JobFlowProject.Business.Dto.JobPost;

namespace JobFlowProject.Business.Interfaces.User;

public interface IAdminService
{
    Task VerifyEmployerAsync(Guid employerId);
    Task<DashboardDto> GetDashboardAsync();
    Task<List<EmployerListDto>> GetEmployersAsync();
    Task RejectEmployerAsync(Guid employerId);
    Task<List<JobPostListDto>> GetJobPostsAsync();

    Task DeleteJobPostAsync(Guid id ,  Guid requesterId);
    Task<List<JobSeekerListDto>> GetJobSeekersAsync();

    Task DeleteJobSeekerAsync(Guid id, Guid requesterId);
}