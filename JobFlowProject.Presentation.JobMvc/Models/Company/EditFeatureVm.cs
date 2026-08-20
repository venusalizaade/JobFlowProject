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

    [StringLength(1000)]
    public string? Description { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(1, 365)]
    public int DurationDays { get; set; }

    public bool IsActive { get; set; } = true;

    [Required(ErrorMessage = "لطفاً نوع فیچر را انتخاب کنید.")]
    public FeatureTypeEnum? FeatureType { get; set; }

    public UpdateFeatureDto ToDto()
    {
        return new UpdateFeatureDto(
            Name,
            Description,
            Price,
            DurationDays,
            IsActive,
            FeatureType!.Value
        );
    }
}
