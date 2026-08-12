using System.ComponentModel.DataAnnotations;
using JobFlowProject.Business.Dto.Commands;

namespace JovFlowProject.JobMvc.Models.Accounts;



    public class RegisterJobSeekerVm
    {
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [StringLength(10)]
    public string NationalId { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(11)]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Gender { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    public RegisterJobSeekerCommand ToCommand()
    {
        return new RegisterJobSeekerCommand(
            FirstName,
            LastName,
            NationalId,
            Email,
            PhoneNumber,
            Password,
            Gender
        );
    }
    }
