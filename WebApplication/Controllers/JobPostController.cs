using System.Security.Claims;
using JobFlowProject.Business.Dto.Commands;
using JobFlowProject.Business.Dto.JobPost;
using JobFlowProject.Business.Interfaces.JobPost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Employer")]
public class JobPostController : ControllerBase
{
    private readonly IJobPostService _jobPostService;

    public JobPostController(IJobPostService jobPostService)
    {
        _jobPostService = jobPostService;
    }

    /// <summary>
    /// ایجاد آگهی شغلی
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(CreateJobPostCommand command)
    {
        var requesterId = GetUserId();

        var result = await _jobPostService.CreateAsync(requesterId, command);

        return Ok(result);
    }

    /// <summary>
    /// لیست آگهی‌های کارفرما
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetMyJobPosts()
    {
        var requesterId = GetUserId();

        var result = await _jobPostService.GetCompanyJobPostsAsync(requesterId);

        return Ok(result);
    }

    /// <summary>
    /// مشاهده جزئیات آگهی
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var result = await _jobPostService.GetDetailsAsync(id);

        return Ok(result);
    }

    /// <summary>
    /// ویرایش آگهی
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateJobPostCommand command)
    {
        var requesterId = GetUserId();

        await _jobPostService.UpdateAsync(requesterId, id, command);

        return NoContent();
    }

    /// <summary>
    /// غیرفعال کردن آگهی
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var requesterId = GetUserId();

        await _jobPostService.DeactivateAsync(requesterId, id);

        return NoContent();
    }
   
    private Guid GetUserId()
    {
        return Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}