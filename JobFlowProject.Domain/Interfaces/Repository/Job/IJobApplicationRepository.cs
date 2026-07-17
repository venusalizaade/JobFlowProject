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
}