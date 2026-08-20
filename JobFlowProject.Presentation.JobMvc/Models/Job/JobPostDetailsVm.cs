using JobFlowProject.Domain.Enums;

namespace JovFlowProject.JobMvc.Models.Job;

public class JobPostDetailsVm
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string AboutJob { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string ProvinceName { get; set; } = string.Empty;
    public string CityName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string SkillName { get; set; } = string.Empty;
    public EmploymentTypeEnum EmploymentType { get; set; }
    public decimal? Salary { get; set; }
    public bool IsFeatured { get; set; }
    public DateTime CreatedAt { get; set; }
}