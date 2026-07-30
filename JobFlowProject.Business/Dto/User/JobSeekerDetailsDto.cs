using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Business.Dto.User;

public record JobSeekerProfileDto
(
    
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string? Gender,
    string? About
);
public record JobSeekerDetailsDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string? Gender,
    string? NationalId,
    string? About,
    List<JobSeekerAttachmentDto?> Attachments,
    List<JobSeekerApplicationDto?> Applications
);


public record JobSeekerAttachmentDto(
    Guid Id,
    string FileName,
    string FilePath
);

public record JobSeekerApplicationDto(
    Guid Id,
    string JobTitle,
    string CompanyName,
    JobApplicationStatusEnum Status,
    DateTime AppliedAt
);