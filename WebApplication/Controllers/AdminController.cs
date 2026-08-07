using System.Security.Claims;
using JobFlowProject.Business.Constants;
using JobFlowProject.Business.Dto.Admin;
using JobFlowProject.Business.Dto.CompanyDto;
using JobFlowProject.Business.Dto.Feature;
using JobFlowProject.Business.Dto.User;
using JobFlowProject.Business.Interfaces.User;
using JobFlowProject.Domain.Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto.Authentication;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")] 
 [Authorize(Roles="Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _service;

    public AdminController(IAdminService service)
    {
        _service = service;
    }

   
    
    [HttpPatch("verify-employer/{id:guid}")]
    [Authorize(Policy = "CanApproveEmployer")]
    public async Task<IActionResult> VerifyEmployer(Guid id)
    {
        await _service.VerifyEmployerAsync(id);

        return Ok(new ApiResponse<object>(
            true,
            null,
            "Operation completed successfully."));
    }
  
    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var result = await _service.GetDashboardAsync();
        var user=User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Ok(new ApiResponse<DashboardDto>(
            true,
            result,
            "Operation completed successfully."));
    }
    
    [HttpGet("employers")]
    public async Task<IActionResult> GetEmployers()
    {
        var result = await _service.GetEmployersAsync();

        return Ok(new ApiResponse<List<EmployerListDto>>(
            true,
            result,
            "Operation completed successfully."));
    }
    
    [HttpPatch("reject-employer/{id:guid}")]
    public async Task<IActionResult> RejectEmployer(Guid id)
    {
        await _service.RejectEmployerAsync(id);

        return Ok(new ApiResponse<object>(
            true,
            null,
            "Employer rejected successfully."));
    }
    [HttpDelete("jobposts/{id:guid}")]
    public async Task<IActionResult> DeleteJobPost(Guid id)
    {
        var requesterId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _service.DeleteJobPostAsync(id, requesterId);

        return Ok(new ApiResponse<object>(
            true,
            null,
            "Job post deleted successfully."));
    }

    [HttpGet("jobseekers")]
    public async Task<IActionResult> GetJobSeekers()
    {
        var result = await _service.GetJobSeekersAsync();

        return Ok(new ApiResponse<List<JobSeekerListDto>>(
            true,
            result,
            "Operation completed successfully."));
    }
    
    [HttpDelete("jobseekers/{id:guid}")]
    public async Task<IActionResult> DeleteJobSeeker(Guid id)
    {
        var requesterId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _service.DeleteJobSeekerAsync(id, requesterId);

        return Ok(new ApiResponse<object>(
            true,
            null,
            "Job seeker deleted successfully."));
    }
    [HttpGet("employers/{id:guid}")]
    public async Task<IActionResult> GetEmployerDetails(Guid id)
    {
        var result = await _service.GetEmployerDetailsAsync(id);

        return Ok(new ApiResponse<EmployerDetailsDto>(
            true,
            result,
            "Operation completed successfully."));
    }

    [HttpGet("jobseekers/{id:guid}")]
    public async Task<IActionResult> GetJobSeekerDetails(Guid id)
    {
        var result = await _service.GetJobSeekerDetailsAsync(id);

        return Ok(new ApiResponse<JobSeekerDetailsDto>(
            true,
            result,
            "Operation completed successfully."));
    }

    [HttpPatch("jobposts/{id:guid}/toggle-status")]
    public async Task<IActionResult> ToggleJobPostStatus(Guid id)
    {
        var requesterId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _service.ToggleJobPostStatusAsync(id, requesterId);

        return Ok(new ApiResponse<object>(
            true,
            null,
            "Job post status updated successfully."));
    }

    [HttpPatch("jobposts/{id:guid}/featured")]
    public async Task<IActionResult> SetJobPostFeatured(
        Guid
            id,
        SetJobPostFeaturedDto dto)
    {
        var requesterId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _service.SetJobPostFeaturedAsync(
            id,
            dto.DurationDays,
            requesterId);

        return Ok(new ApiResponse<object>(
            true,
            null,
            "Job post marked as featured successfully."));
    }

    [HttpDelete("jobposts/{id:guid}/featured")]
    public async Task<IActionResult> RemoveJobPostFeatured(Guid id)
    {
        var requesterId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _service.RemoveJobPostFeaturedAsync(
            id,
            requesterId);

        return Ok(new ApiResponse<object>(
            true,
            null,
            "Featured status removed successfully."));
    }
    [HttpPatch("jobseekers/{id:guid}/disable")]
    public async Task<IActionResult> DisableJobSeeker(Guid id)
    {
        await _service.DisableJobSeekerAsync(id);

        return Ok(new ApiResponse<object>(
            true,
            null,
            "Job seeker account disabled successfully."));
    }

    [HttpPatch("jobseekers/{id:guid}/enable")]
    public async Task<IActionResult> EnableJobSeeker(Guid id)
    {
        await _service.EnableJobSeekerAsync(id);

        return Ok(new ApiResponse<object>(
            true,
            null,
            "Job seeker account enabled successfully."));
    }
    
    [HttpGet("email-settings")]
    public async Task<IActionResult> GetEmailSettings()
    {
        var result = await _service.GetEmailSettingAsync();

        return Ok(new ApiResponse<object>(
            true,
            result,
            null));
    }

    [HttpPut("email-settings")]
    public async Task<IActionResult> UpdateEmailSettings(UpdateEmailSettingDto dto)
    {
        await _service.UpdateEmailSettingAsync(dto);

        return Ok(new ApiResponse<object>(
            true,
            null,
            "Email settings updated successfully."));
    }

}
