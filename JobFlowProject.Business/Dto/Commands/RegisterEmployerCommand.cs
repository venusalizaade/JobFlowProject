
using JobFlowProject.Domain.Entities.Componies;
namespace JobFlowProject.Business.Dto.Commands;

public record RegisterEmployerCommand(
    string FirstName,
    string LastName,
    string NationalId,
    string Password,
    string Email,
    string PhoneNumber,
    string? Gender,
    string CompanyName,
    string CompanyNationalId,
    string City,
    string Province,
    string Address
 
);