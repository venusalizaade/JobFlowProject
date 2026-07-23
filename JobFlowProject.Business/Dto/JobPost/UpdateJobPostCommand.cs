using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Business.Dto.JobPost;

public record UpdateJobPostCommand(
    string Title,
    string AboutJob,
    Guid ProvinceId,
    Guid CityId,
    Guid CategoryId,
    EmploymentTypeEnum EmploymentType,
    decimal? Salary,
    Guid SkillId
);
