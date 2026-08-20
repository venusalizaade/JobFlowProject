using System.ComponentModel.DataAnnotations;
using JobFlowProject.Business.Dto.Commands;
using JobFlowProject.Domain.Enums;

namespace JovFlowProject.JobMvc.Models.Job;

public class ChangeApplicationStatusVm
{
    [Required]
    public Guid JobApplicationId { get; set; }

    [Required]
    public Guid JobPostId { get; set; }

    [Required]
    public JobApplicationStatusEnum Status { get; set; }

    public ChangeApplicationStatusCommand ToCommand()
    {
        return new ChangeApplicationStatusCommand(
            JobApplicationId,
            JobPostId,
            Status
        );
    }
}