using System.Security.Claims;
using JobFlowProject.Business.Constants;
using JobFlowProject.Business.Dto.Admin;
using JobFlowProject.Business.Interfaces.User;
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
}
