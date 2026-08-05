using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Business.Dto.JobPost;

public record JobPostSearchRequestDto(
    string? Title,
    Guid? CategoryId,
    Guid? SkillId,
    EmploymentTypeEnum? EmploymentType,
    decimal? MinSalary,
    decimal? MaxSalary,
    Guid? CityId,
    Guid? ProvinceId
);
