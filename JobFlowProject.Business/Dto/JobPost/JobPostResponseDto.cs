using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Business.Dto.JobPost;


public record JobPostResponseDto(
    Guid Id,
    string Title,
    string AboutJob,
    decimal? Salary,
    EmploymentTypeEnum EmploymentType,
    bool IsActive,
    DateTime ExpiresAt
)
{
    public bool IsFeatured { get; set; }
}