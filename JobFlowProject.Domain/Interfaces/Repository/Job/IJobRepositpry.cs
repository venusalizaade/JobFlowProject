using JobFlowProject.Domain.Entities.Job;

namespace JobFlowProject.Domain.Interfaces.Repository;

public interface IJobPostRepository : IGenericRepository<JobPost>
{
    Task<List<JobPost>> GetCompanyJobPostsAsync(Guid companyId);

    Task<JobPost?> GetJobPostDetailsAsync(Guid id);
    Task<List<JobPost>> GetActiveAsync();
    
 
}