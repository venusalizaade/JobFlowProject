namespace JobFlowProject.Business.Dto.Admin;

public record JobSeekerListDto(
    Guid Id,
    string FullName,
    string Email,
    string PhoneNumber,
    string? Gender,
    string? About
);