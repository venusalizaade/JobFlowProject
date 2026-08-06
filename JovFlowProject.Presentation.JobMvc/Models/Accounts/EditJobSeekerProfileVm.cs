using System.ComponentModel.DataAnnotations;
using JobFlowProject.Business.Dto.User;

namespace JovFlowProject.JobMvc.Models.Accounts;

public class EditJobSeekerProfileVm
{
    
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [StringLength(11)]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Gender { get; set; }

    [StringLength(2000)]
    public string? About { get; set; }

    public UpdateJobSeekerProfileDto ToDto()
    {
        return new UpdateJobSeekerProfileDto(
            FirstName,
            LastName,
            PhoneNumber,
            Gender,
            About
        );
    }
}