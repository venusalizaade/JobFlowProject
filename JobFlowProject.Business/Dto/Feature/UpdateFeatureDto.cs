using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Business.Dto.Feature;

public record UpdateFeatureDto(
    string Name,
    decimal Price,
    int DurationDays,
    FeatureTypeEnum FeatureType
);