using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Business.Dto.Feature;

public record FeatureListDto(
    Guid Id,
    string Name,
    decimal Price,
    int DurationDays
)
{
    public FeatureTypeEnum FeatureType { get; set; }
}