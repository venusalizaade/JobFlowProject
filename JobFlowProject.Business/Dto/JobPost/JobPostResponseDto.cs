using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Business.Dto.JobPost;


public record JobPostResponseDto(
    Guid Id,
    string Title,
    string AboutJob,
    decimal? Salary,
    EmploymentTypeEnum EmploymentType,
    bool IsActive,
    DateTime ExpiresAt
)
{
    public bool IsFeatured { get; set; }
    public string? CompanyName { get; set; }
    public string? CategoryName { get; set; }
    public string? CityName { get; set; }
    public string? ProvinceName { get; set; }
    public string? SkillName { get; set; }
    public DateTime? CreatedAt { get; set; }
}