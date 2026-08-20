using System.ComponentModel.DataAnnotations;
using JobFlowProject.Business.Dto.User;

namespace JovFlowProject.JobMvc.Models.Accounts;

public class EditJobSeekerProfileVm
{
    [Required(ErrorMessage = "نام الزامی است.")]
    [Display(Name = "نام")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "نام باید بین ۲ تا ۱۰۰ کاراکتر باشد.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "نام خانوادگی الزامی است.")]
    [Display(Name = "نام خانوادگی")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "نام خانوادگی باید بین ۲ تا ۱۰۰ کاراکتر باشد.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "شماره موبایل الزامی است.")]
    [Display(Name = "شماره موبایل")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "شماره موبایل باید ۱۱ رقم باشد.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Display(Name = "جنسیت")]
    [StringLength(20)]
    public string? Gender { get; set; }

    [Display(Name = "درباره من")]
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
