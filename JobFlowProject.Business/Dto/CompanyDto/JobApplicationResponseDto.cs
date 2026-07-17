using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Business.Dto.CompanyDto;

public record JobApplicationResponseDto(
    Guid Id,
    Guid JobPostId,
    Guid ApplicantId,
    string ApplicantName,
    ApplicationStatusEnum Status
);