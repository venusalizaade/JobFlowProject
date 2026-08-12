using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Business.Dto.JobPost;

public record JobPostDetailDto(
    Guid Id,
    string Title,
    string AboutJob,
    decimal? Salary,
    EmploymentTypeEnum EmploymentType,
    bool IsActive,
    DateTime ExpiresAt,

    string CompanyName,
    string CategoryName,
    string CityName,
    string ProvinceName,

    string CompanyAddress,
    string CompanyLogoUrl
);
