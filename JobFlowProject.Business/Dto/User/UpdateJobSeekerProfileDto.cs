namespace JobFlowProject.Business.Dto.User;

public record UpdateJobSeekerProfileDto
(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string? Gender,
    string? About
);