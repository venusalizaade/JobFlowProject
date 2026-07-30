using System.Security.Claims;
using JobFlowProject.Business.Dto.Commands;
using JobFlowProject.Business.Interfaces.JobPost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto.Authentication;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/employer/job-applications")]
[Authorize(Policy = "ApprovedEmployer")]
public class JobApplicationEmployerController : ControllerBase
{
    private readonly IJobApplicationService _service;

            
    public JobApplicationEmployerController(IJobApplicationService service)
    {
        _service = service;
    }

    [HttpGet("job-post/{jobPostId:guid}")]
    public async Task<IActionResult> GetJobApplications(Guid jobPostId)
    {
        var requesterId = GetUserId();

        var result = await _service.GetJobApplicationsAsync(
            requesterId,
            jobPostId);

        return Ok(new ApiResponse<object>(
            true,
            result,
            null));
    }

    [HttpPatch("status")]
    public async Task<IActionResult> ChangeStatus(ChangeApplicationStatusCommand command)
    {
        var requesterId = GetUserId();

        await _service.ChangeStatusAsync(
            requesterId,
            command);

        return Ok(new ApiResponse<object>(
            true,
            null,
            "Application status updated successfully."));
    }

    private Guid GetUserId()
    {
        return Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
    

}