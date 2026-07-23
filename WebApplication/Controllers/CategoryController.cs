using JobFlowProject.Business.Interfaces.EmployerInterfaces;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto.Authentication;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result= await _service.GetAllAsync();
        return Ok(new ApiResponse<object>(
            true,
            result,
            null));
    }
}