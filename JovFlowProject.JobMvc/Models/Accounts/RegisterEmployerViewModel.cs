using System.ComponentModel.DataAnnotations;
using JobFlowProject.Business.Dto.Commands;
using JobFlowProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JovFlowProject.JobMvc.Models.Accounts;

public class RegisterEmployerViewModel
{

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 100 characters.")]
    public string FirstName { get; set; } = string.Empty;


    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 100 characters.")]
    public string LastName { get; set; } = string.Empty;


    [Required(ErrorMessage = "National ID is required.")]
    [StringLength(10, ErrorMessage = "National ID must contain exactly 10 digits.")]
    public string NationalId { get; set; } = string.Empty;


    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;


    [Required(ErrorMessage = "Phone number is required.")]
    [StringLength(11, ErrorMessage = "Phone number format is invalid.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(20)] public string? Gender { get; set; }


    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; } = string.Empty;


    [Required(ErrorMessage = "Company name is required.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Company name must be between 2 and 200 characters.")]
    public string CompanyName { get; set; } = string.Empty;


    [Required(ErrorMessage = "Company national ID is required.")]
    [StringLength(10)]
    public string CompanyNationalId { get; set; } = string.Empty;


    [Required(ErrorMessage = "Company address is required.")]
    [StringLength(500)]
    public string Address { get; set; } = string.Empty;

    [StringLength(2000)] public string? CompanyAbout { get; set; }


    [Required(ErrorMessage = "Province is required.")]
    public Guid ProvinceId { get; set; }


    [Required(ErrorMessage = "City is required.")]
    public Guid CityId { get; set; }

    public RegisterEmployerCommand ToCommand()
    {
        return new RegisterEmployerCommand(
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
            Address
        );
    }
}
    