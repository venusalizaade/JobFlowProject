using JobFlowProject.Domain.Entities.User;

namespace JobFlowProject.Domain.Interfaces.Repository.User;

public interface IAdminRepository

{
    Task<AppUser?> GetEmployerDetailsAsync(Guid employerId);

    Task<AppUser?> GetJobSeekerDetailsAsync(Guid jobSeekerId);

    Task<int> GetTotalCompaniesAsync();

    Task<int> GetTotalJobPostsAsync();

    Task<int> GetPendingApplicationsAsync();

}