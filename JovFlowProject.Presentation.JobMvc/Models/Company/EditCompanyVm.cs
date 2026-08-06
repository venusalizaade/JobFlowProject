using System.ComponentModel.DataAnnotations;
using JobFlowProject.Business.Dto.CompanyDto;

namespace JovFlowProject.JobMvc.Models.Company;

public class EditCompanyVm
{
    [Required]
    [StringLength(200, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public Guid ProvinceId { get; set; }

    [Required]
    public Guid CityId { get; set; }

    [Required]
    [StringLength(500)]
    public string Address { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? About { get; set; }

    public UpdateCompanyRequestDto ToDto()
    {
        return new UpdateCompanyRequestDto(
            Name,
            ProvinceId,
            CityId,
            Address,
            About
        );
    }
}