using JobFlowProject.Domain.Enums;

namespace WebApplication1.Dto.Authentication;

public record JobPostSearchRequestDto(
    string? Title,
    EmploymentTypeEnum? EmploymentType,
    Guid? CityId
);