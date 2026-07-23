using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Business.Dto.JobPost;
public record JobPostDetailsDto(
    Guid Id,
    string Title,
    string AboutJob,
    decimal? Salary,
    EmploymentTypeEnum EmploymentType,
    DateTime ExpiresAt,
    string CompanyName,
    string CategoryName,
    string SkillName,
    string CityName,
    string ProvinceName
);