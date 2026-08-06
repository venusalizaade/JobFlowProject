using System.ComponentModel.DataAnnotations;
using JobFlowProject.Business.Dto.Feature;
using JobFlowProject.Domain.Enums;

namespace JovFlowProject.JobMvc.Models.Company;

public class EditFeatureVm
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(1, 365)]
    public int DurationDays { get; set; }

    [Required]
    public FeatureTypeEnum FeatureType { get; set; }

    public UpdateFeatureDto ToDto()
    {
        return new UpdateFeatureDto(
            Name,
            Price,
            DurationDays,
            FeatureType
        );
    }
}