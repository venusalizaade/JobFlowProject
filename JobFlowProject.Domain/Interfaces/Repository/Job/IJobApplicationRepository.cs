using JobFlowProject.Domain.Entities.Job;

namespace JobFlowProject.Domain.Interfaces.Repository;

public interface IJobApplicationRepository 
    : IGenericRepository<JobApplication>
{
    Task<bool> ExistsAsync(
        Guid jobPostId,
        Guid applicantId);

    Task<List<JobApplication>> GetByJobPostAsync(
        Guid jobPostId);
    
    Task<bool> HasAppliedAsync(Guid jobPostId, Guid jobSeekerId);

    Task<List<JobApplication>> GetByJobSeekerIdAsync(Guid jobSeekerId);

    Task<JobApplication?> GetDetailsAsync(Guid applicationId);

    Task<JobApplication?> GetPendingApplicationAsync(Guid applicationId, Guid jobSeekerId);
}