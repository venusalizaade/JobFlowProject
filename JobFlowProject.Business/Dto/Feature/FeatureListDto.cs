using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Business.Dto.Feature;

public record FeatureListDto(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    int DurationDays,
    bool IsActive,
    FeatureTypeEnum FeatureType
);
