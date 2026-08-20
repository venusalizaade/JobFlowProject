using System.Diagnostics;
using JobFlowProject.Business.Interfaces.EmployerInterfaces;
using JobFlowProject.Business.Interfaces.JobPost;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using JovFlowProject.JobMvc.Models;

namespace JovFlowProject.JobMvc.Controllers;

public class HomeController : Controller
{
    private readonly IJobPostService _jobPostService;
    private readonly ICategoryService _categoryService;
    private readonly IMemoryCache _cache;

    public HomeController(IJobPostService jobPostService, ICategoryService categoryService, IMemoryCache cache)
    {
        _jobPostService = jobPostService;
        _categoryService = categoryService;
        _cache = cache;
    }

    public async Task<IActionResult> Index()
    {
        const string cacheKey = "HomeIndex:JobsAndCategories";

        if (!_cache.TryGetValue(cacheKey, out HomeIndexVm? model))
        {
            model = new HomeIndexVm();

            try
            {
                model.Jobs = await _jobPostService.GetActiveAsync();
                model.Categories = await _categoryService.GetAllAsync();
            }
            catch
            {
                model.Jobs = new List<JobFlowProject.Business.Dto.JobPost.JobPostResponseDto>();
                model.Categories = new List<JobFlowProject.Business.Dto.CompanyDto.CategoryResponseDto>();
            }

            _cache.Set(cacheKey, model, TimeSpan.FromMinutes(5));
        }

        return View(model);
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
