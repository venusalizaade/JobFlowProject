namespace JobFlowProject.Business.Dto.User;

public record JobSeekerProfileDto(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string? Gender,
    string? About
    );