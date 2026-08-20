using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Business.Dto.Feature;

public record CreateFeatureDto(
    string Name,
    string? Description,
    decimal Price,
    int DurationDays,
    FeatureTypeEnum FeatureType
);
