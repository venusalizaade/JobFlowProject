using System.ComponentModel.DataAnnotations;
using JobFlowProject.Business.Dto.Feature;
using JobFlowProject.Domain.Enums;

namespace JovFlowProject.JobMvc.Models.Company;

public class CreateFeatureVm
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(1, 365)]
    public int DurationDays { get; set; }

    [Required]
    public FeatureTypeEnum FeatureType { get; set; }

    public CreateFeatureDto ToDto()
    {
        return new CreateFeatureDto(
            Name,
            Price,
            DurationDays,
            FeatureType
        );
    }
}
