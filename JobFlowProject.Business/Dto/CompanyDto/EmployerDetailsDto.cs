namespace JobFlowProject.Business.Dto.CompanyDto;

public record EmployerDetailsDto(
    Guid Id,
    string FullName,
    string Email,
    string PhoneNumber,
    bool IsApproved,
    string? CompanyName,
    string? CompanyNationalId,
    string? CompanyAddress,
    string? CompanyAbout
);