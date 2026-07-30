namespace JobFlowProject.Business.Dto.Feature;

public record AssignFeatureToCompanyDto(
    Guid CompanyId,
    Guid FeatureId
);