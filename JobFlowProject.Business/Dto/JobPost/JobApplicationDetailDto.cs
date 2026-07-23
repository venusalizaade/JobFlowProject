using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Business.Dto.JobPost;

public record JobApplicationDetailDto(
    Guid Id,
    string JobTitle,
    string CompanyName,
    JobApplicationStatusEnum Status,
    DateTime CreatedAt
);