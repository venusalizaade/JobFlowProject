using JobFlowProject.Business.Dto.Commands;

namespace WebApplication1.Dto.Authentication;

public record RegisterJobSeekerRequestDto(
    string FirstName,
    string LastName,
    string NationalId,
    string Email,
    string PhoneNumber,
    string Gender,
    string Password)
{
    public RegisterJobSeekerCommand ToCommand()
        => new(
            FirstName,
            LastName,
            NationalId,
            Email,
            PhoneNumber,
            Gender,
            Password);
}
