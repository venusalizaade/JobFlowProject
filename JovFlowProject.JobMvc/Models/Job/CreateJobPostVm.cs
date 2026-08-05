using System.ComponentModel.DataAnnotations;
using JobFlowProject.Business.Dto.Commands;
using JobFlowProject.Domain.Enums;

namespace JovFlowProject.JobMvc.Models.Job;

public class CreateJobPostVm
{
    [Required]
    [StringLength(200, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(5000, MinimumLength = 10)]
    public string AboutJob { get; set; } = string.Empty;

    [Required]
    public Guid ProvinceId { get; set; }

    [Required]
    public Guid CityId { get; set; }

    [Required]
    public EmploymentTypeEnum EmploymentType { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? Salary { get; set; }

    [Required]
    public Guid CategoryId { get; set; }

    [Required]
    public Guid SkillId { get; set; }

    public CreateJobPostCommand ToCommand()
    {
        return new CreateJobPostCommand(
            Title,
            AboutJob,
            ProvinceId,
            CityId,
            Salary,
            EmploymentType,
            CategoryId, 
            SkillId
        );
    }
}