using System.Security.Claims;
using JobFlowProject.Business.Dto.User;
using JobFlowProject.Business.Interfaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto.Authentication;

namespace WebApplication1.Controllers;



[ApiController]
[Route("api/jobseeker")]
[Authorize(Roles = "JobSeeker")]
public class JobSeekerController : ControllerBase
{
    private readonly IJobSeekerService _service;

    public JobSeekerController(IJobSeekerService service)
    {
        _service = service;
    }

    private Guid GetUserId()
    {
        return  Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var result = await _service.GetProfileAsync(GetUserId());

        return Ok(new ApiResponse<object>(
            true,
            result,
            null));
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(UpdateJobSeekerProfileDto dto)
    {
        await _service.UpdateProfileAsync(GetUserId(), dto);

        return Ok(new ApiResponse<object>(
            true,
            null,
            "Operation completed successfully."));
    }

    [HttpPost("resume")]
    public async Task<IActionResult> UploadResume([FromForm] IFormFile file)
    {
        await _service.UploadResumeAsync(GetUserId(), file);

        return Ok(new ApiResponse<object>(
            true,
            null,
            "Resume uploaded successfully."));
    }

    [HttpPut("resume")]
    public async Task<IActionResult> ReplaceResume([FromForm] IFormFile file)
    {
        await _service.ReplaceResumeAsync(GetUserId(), file);

        return Ok(new ApiResponse<object>(
            true,
            null,
            "Operation completed successfully."));
    }

    [HttpDelete("resume")]
    public async Task<IActionResult> DeleteResume()
    {
        await _service.DeleteResumeAsync(GetUserId());

        return Ok(new ApiResponse<object>(
            true,
            null,
            "Operation completed successfully."));
    }
    
    [HttpGet("resume")]
    public async Task<IActionResult> GetResume()
    {
        var resume = await _service.GetResumeAsync(GetUserId());

        var bytes = await System.IO.File.ReadAllBytesAsync(resume.FilePath);

        return File(
            bytes,
            resume.FileType,
            resume.FileName);
    }
}