namespace JobFlowProject.Business.Dto.Feature;

public record FeatureListDto(
    Guid Id,
    string Name,
    decimal Price,
    int DurationDays
);