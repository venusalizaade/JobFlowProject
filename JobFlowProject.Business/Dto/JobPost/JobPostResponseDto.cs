using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Business.Dto.JobPost;


public record JobPostResponseDto(
    Guid Id,
    string Title,
    string AboutJob,
    string? Salary,
    EmploymentTypeEnum EmploymentType,
    bool IsActive,
    DateTime ExpiresAt
);