using System.Security.Claims;
using JobFlowProject.Business.Dto.Commands;
using JobFlowProject.Business.Dto.JobPost;
using JobFlowProject.Business.Interfaces.JobPost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobPostController : ControllerBase
{
    private readonly IJobPostService _jobPostService;

    public JobPostController(IJobPostService jobPostService)
    {
        _jobPostService = jobPostService;
    }

    [Authorize(Policy = "ApprovedEmployer")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateJobPostCommand command)
    {
        var requesterId = GetUserId();

        var result = await _jobPostService.CreateAsync(requesterId, command);

        return Ok(result);
    }

    [Authorize(Policy = "ApprovedEmployer")]
    [HttpGet]
    public async Task<IActionResult> GetMyJobPosts()
    {
        var requesterId = GetUserId();

        var result = await _jobPostService.GetCompanyJobPostsAsync(requesterId);

        return Ok(result);
    }

   
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var result = await _jobPostService.GetDetailsAsync(id);

        return Ok(result);
    }

    [Authorize(Policy = "ApprovedEmployer")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateJobPostCommand command)
    {
        var requesterId = GetUserId();

        await _jobPostService.UpdateAsync(requesterId, id, command);

        return NoContent();
    }

    [Authorize(Policy = "ApprovedEmployer")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var requesterId = GetUserId();

        await _jobPostService.DeactivateAsync(requesterId, id);

        return NoContent();
    }
    
    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        return Ok(await _jobPostService.GetActiveAsync());
    }
   
    private Guid GetUserId()
    {
        return Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}