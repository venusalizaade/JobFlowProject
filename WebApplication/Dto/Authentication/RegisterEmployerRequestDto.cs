using JobFlowProject.Business.Dto.Commands;

namespace WebApplication1.Dto.Authentication;

public record RegisterEmployerRequestDto(
    string FirstName,
    string LastName,
    string NationalId,
    string Email,
    string PhoneNumber,
    string Gender,
    string Password,
    string CompanyName,
    string CompanyNationalId,
    Guid CityId,
    Guid ProvinceId,
    string Address)
{
    public RegisterEmployerCommand ToCommand()
        => new(
             FirstName,
            LastName,
            NationalId,
             Password,
             Email,
             PhoneNumber,
             Gender,
             CompanyName,
             CompanyNationalId,
             CityId,
             ProvinceId,
             Address);
}


