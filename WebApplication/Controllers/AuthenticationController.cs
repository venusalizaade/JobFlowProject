using JobFlowProject.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Authentication.RequestDtos;
using WebApplication1.Dto.Authentication;

namespace WebApplication1.Controllers;


[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("jobseeker/register")]
    public async Task<IActionResult> RegisterJobSeeker([FromBody] RegisterJobSeekerRequestDto request)
    {
        await _authenticationService.JobSeekerRegisterAsync(request.ToCommand());
        return StatusCode(200, new { message = "JobSeeker registered successfully." });
    }

    [HttpPost("employer/register")]
    public async Task<IActionResult> RegisterEmployer([FromBody] RegisterEmployerRequestDto request)
    {
        await _authenticationService.EmployerRegisterAsync(request.ToCommand());
        return StatusCode(200, new { message = "Employer registered successfully." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
       
        var token = await _authenticationService.LoginAsync(request.ToCommand());
        return Ok(token);
    }
}