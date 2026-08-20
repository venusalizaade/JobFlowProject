using System.ComponentModel.DataAnnotations;
using JobFlowProject.Business.Dto.Commands;
using JobFlowProject.Domain.Enums;

namespace JovFlowProject.JobMvc.Models.Job;

public class CreateJobPostVm
{
    [Display(Name = "عنوان شغل")]
    [Required(ErrorMessage = "عنوان شغل الزامی است.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "عنوان شغل باید بین ۳ تا ۲۰۰ کاراکتر باشد.")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "شرح شغل")]
    [Required(ErrorMessage = "شرح شغل الزامی است.")]
    [StringLength(5000, MinimumLength = 10, ErrorMessage = "شرح شغل باید حداقل ۱۰ کاراکتر باشد.")]
    public string AboutJob { get; set; } = string.Empty;

    [Display(Name = "استان")]
    [Required(ErrorMessage = "استان را انتخاب کنید.")]
    public Guid? ProvinceId { get; set; }

    [Display(Name = "شهر")]
    [Required(ErrorMessage = "شهر را انتخاب کنید.")]
    public Guid? CityId { get; set; }

    [Display(Name = "نوع همکاری")]
    [Required(ErrorMessage = "نوع همکاری را انتخاب کنید.")]
    public EmploymentTypeEnum? EmploymentType { get; set; }

    [Display(Name = "حقوق ماهانه (تومان)")]
    [Range(0, double.MaxValue, ErrorMessage = "مقدار حقوق معتبر نیست.")]
    public decimal? Salary { get; set; }

    [Display(Name = "دسته‌بندی")]
    [Required(ErrorMessage = "دسته‌بندی را انتخاب کنید.")]
    public Guid? CategoryId { get; set; }

    [Display(Name = "مهارت")]
    [Required(ErrorMessage = "مهارت را انتخاب کنید.")]
    public Guid? SkillId { get; set; }

    [Display(Name = "فیچر")]
    public Guid? FeatureId { get; set; }

    public CreateJobPostCommand ToCommand()
    {
        return new CreateJobPostCommand(
            Title,
            AboutJob,
            ProvinceId ?? Guid.Empty,
            CityId ?? Guid.Empty,
            Salary,
            EmploymentType ?? EmploymentTypeEnum.FullTime,
            CategoryId ?? Guid.Empty,
            SkillId ?? Guid.Empty
        );
    }
}