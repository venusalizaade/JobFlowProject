namespace JobFlowProject.Business.Dto.Admin;

public record EmployerListDto(
    Guid Id,
    string FullName,
    string Email,
    string PhoneNumber,
    string? CompanyName,
    bool IsApproved
);