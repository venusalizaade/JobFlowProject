using JobFlowProject.Domain.Entities.Componies;
namespace JobFlowProject.Business.Dto.Commands;

public record RegisterJobSeekerCommand(
    string FirstName,
    string LastName,
    string NationalId,
    string Email,
    string PhoneNumber,
    string Password,
    string? Gender = null
    );
