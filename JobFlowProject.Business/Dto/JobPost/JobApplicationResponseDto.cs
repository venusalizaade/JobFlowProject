using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Business.Dto.JobPost;

public record JobApplicationResponseDto(
    Guid Id,
    string JobTitle,
    JobApplicationStatusEnum Status,
    DateTime CreatedAt
);