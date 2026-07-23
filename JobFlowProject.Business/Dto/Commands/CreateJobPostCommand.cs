using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Business.Dto.Commands;

public record CreateJobPostCommand(
    string Title,
    string AboutJob,
    Guid ProvinceId,
    Guid CityId,
    decimal? Salary,
    EmploymentTypeEnum EmploymentType,
    Guid CategoryId,
    Guid SkillId
);