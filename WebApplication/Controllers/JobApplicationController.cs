using System.Security.Claims;
using JobFlowProject.Business.Dto.Commands;
using JobFlowProject.Business.Interfaces.EmployerInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobFlowProject.Business.Interfaces.JobPost;
using static JobFlowProject.Business.Interfaces.EmployerInterfaces.IJobApplicationService;
using IJobApplicationService = JobFlowProject.Business.Interfaces.JobPost.IJobApplicationService;


namespace WebApplication1.Controllers;


[ApiController]
[Route("api/[controller]")]
public class JobApplicationController : ControllerBase
{
    private readonly IJobApplicationService _service;

    public JobApplicationController(IJobApplicationService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "JobSeeker")]
    public async Task<IActionResult> Apply(ApplyJobCommand command)
    {
        var requesterId = GetUserId();
        await _service.ApplyAsync(requesterId, command);
        return Ok(new { message = "Application submitted successfully" });
    }

    [HttpGet("job-post/{jobPostId:guid}")]
    [Authorize(Roles = "Employer")]
    public async Task<IActionResult> GetJobApplications(Guid jobPostId)
    {
        var requesterId = GetUserId();
        var result = await _service.GetJobApplicationsAsync(requesterId, jobPostId);
        return Ok(result);
    }

    [HttpPatch("status")]
    [Authorize(Roles = "Employer")]
    public async Task<IActionResult> ChangeStatus(ChangeApplicationStatusCommand command)
    {
        var requesterId = GetUserId();
        await _service.ChangeStatusAsync(requesterId, command);
        return NoContent();
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return Guid.Parse(claim!.Value);
    }
}