using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Business.Dto.CompanyDto;

public record JobApplicationDto(
    Guid Id,
    Guid JobPostId,
    Guid ApplicantId,
    string ApplicantName,
    JobApplicationStatusEnum Status
);
