using JobFlowProject.Business.Constants;
using JobFlowProject.Business.Interfaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")] 
[Authorize(Roles = "Admin")]
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
        
        return NoContent();
    }
}
