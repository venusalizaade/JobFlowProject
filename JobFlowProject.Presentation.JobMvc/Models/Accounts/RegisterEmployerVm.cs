using System.ComponentModel.DataAnnotations;
using JobFlowProject.Business.Dto.Commands;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JovFlowProject.JobMvc.Models.Accounts;

public class RegisterEmployerVm
{
    [Required(ErrorMessage = "نام الزامی است.")]
    [Display(Name = "نام")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "نام باید بین ۲ تا ۱۰۰ کاراکتر باشد.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "نام خانوادگی الزامی است.")]
    [Display(Name = "نام خانوادگی")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "نام خانوادگی باید بین ۲ تا ۱۰۰ کاراکتر باشد.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "کد ملی الزامی است.")]
    [Display(Name = "کد ملی")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "کد ملی باید دقیقاً ۱۰ رقم باشد.")]
    public string NationalId { get; set; } = string.Empty;

    [Required(ErrorMessage = "ایمیل الزامی است.")]
    [EmailAddress(ErrorMessage = "فرمت ایمیل نامعتبر است.")]
    [Display(Name = "ایمیل")]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "شماره موبایل الزامی است.")]
    [Display(Name = "شماره موبایل")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "شماره موبایل باید ۱۱ رقم باشد.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Display(Name = "جنسیت")]
    [StringLength(20)]
    public string? Gender { get; set; }

    [Required(ErrorMessage = "رمز عبور الزامی است.")]
    [Display(Name = "رمز عبور")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "رمز عبور باید حداقل ۶ کاراکتر باشد.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "نام شرکت الزامی است.")]
    [Display(Name = "نام شرکت")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "نام شرکت باید بین ۲ تا ۲۰۰ کاراکتر باشد.")]
    public string CompanyName { get; set; } = string.Empty;

    [Required(ErrorMessage = "شناسه ملی شرکت الزامی است.")]
    [Display(Name = "شناسه ملی شرکت")]
    [StringLength(10)]
    public string CompanyNationalId { get; set; } = string.Empty;

    [Required(ErrorMessage = "آدرس الزامی است.")]
    [Display(Name = "آدرس")]
    [StringLength(500)]
    public string Address { get; set; } = string.Empty;

    [Display(Name = "درباره شرکت")]
    [StringLength(2000)]
    public string? CompanyAbout { get; set; }

    [Required(ErrorMessage = "استان الزامی است.")]
    [Display(Name = "استان")]
    public Guid ProvinceId { get; set; }

    [Required(ErrorMessage = "شهر الزامی است.")]
    [Display(Name = "شهر")]
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
