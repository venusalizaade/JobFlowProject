using JobFlowProject.Business.Interfaces.EmployerInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace JovFlowProject.JobMvc.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAllAsync();
        return View(categories);
    }
}
