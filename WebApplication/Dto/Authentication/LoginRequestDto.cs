using JobFlowProject.Business.Dto.Commands;

namespace WebApplication1.Authentication.RequestDtos;

public record LoginRequestDto(
    string NationalId,
    string Password)
{
    public LoginCommand ToCommand()
        => new(NationalId, Password);
}