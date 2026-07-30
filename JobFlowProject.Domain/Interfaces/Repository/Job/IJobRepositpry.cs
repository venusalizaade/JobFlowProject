using JobFlowProject.Domain.Entities.Job;
using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Domain.Interfaces.Repository;

public interface IJobPostRepository : IGenericRepository<JobPost>
{
    Task<List<JobPost>> GetCompanyJobPostsAsync(Guid companyId);

    Task<JobPost?> GetJobPostDetailsAsync(Guid id);
    Task<List<JobPost>> GetActiveAsync();
    
    Task<List<JobPost>> SearchAsync(string? title, Guid? categoryId, Guid? skillId,
        EmploymentTypeEnum? employmentType, decimal? minSalary, decimal? maxSalary,
        Guid? cityId, Guid? provinceId);
    
    Task<List<JobPost>> SearchAsync(string? title, EmploymentTypeEnum? employmentType, Guid? cityId);
    
    Task<List<JobPost>> FilterAsync(Guid? categoryId, Guid? skillId, decimal? minSalary, decimal? maxSalary);
    
    Task<List<JobPost>> GetAllAsync();
    Task ToggleActiveAsync(Guid jobPostId, Guid requesterId);
    
    Task SetFeaturedAsync(Guid jobPostId, int durationDays, Guid requesterId);

    Task RemoveFeaturedAsync(Guid jobPostId, Guid requesterId);
}