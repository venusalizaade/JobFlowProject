using System.ComponentModel.DataAnnotations;
using JobFlowProject.Business.Dto.Commands;

namespace JovFlowProject.JobMvc.Models.Job;

public class CreateJobApplicationVm
{
    [Required] public Guid JobPostId { get; set; }

    public ApplyJobCommand ToCommand()
    {
        return new ApplyJobCommand(JobPostId);
    }
}