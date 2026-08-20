using System.ComponentModel.DataAnnotations;
using JobFlowProject.Business.Dto.CompanyDto;

namespace JovFlowProject.JobMvc.Models.Company;

public class EditCompanyVm
{
    public Guid CompanyId { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public Guid? ProvinceId { get; set; }

    [Required(ErrorMessage = "لطفاً شهر را انتخاب کنید.")]
    public Guid? CityId { get; set; }

    [Required(ErrorMessage = "لطفاً آدرس را وارد کنید.")]
    [StringLength(500)]
    public string Address { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? About { get; set; }

    [Required(ErrorMessage = "ایمیل الزامی است.")]
    [EmailAddress(ErrorMessage = "ایمیل معتبر نیست.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "شماره تماس الزامی است.")]
    public string PhoneNumber { get; set; } = string.Empty;

    public UpdateCompanyRequestDto ToDto()
    {
        return new UpdateCompanyRequestDto(
            Name,
            ProvinceId ?? Guid.Empty,
            CityId ?? Guid.Empty,
            Address,
            About
        );
    }
}