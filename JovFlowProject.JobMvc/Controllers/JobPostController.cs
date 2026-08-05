using System.Security.Claims;
using JobFlowProject.Business.Dto.JobPost;
using JobFlowProject.Business.Interfaces.JobPost;
using JovFlowProject.JobMvc.Models.Job;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto.Authentication;

namespace JovFlowProject.JobMvc.Controllers;

public class JobPostController : Controller
{
    private readonly IJobPostService _jobPostService;

    public JobPostController(IJobPostService jobPostService)
    {
        _jobPostService = jobPostService;
    }

    public async Task<IActionResult> Index()
    {
        var jobs = await _jobPostService.GetActiveAsync();
        return View(jobs);
    }

    public async Task<IActionResult> MyJobs()
    {
        var requesterId = GetUserId();
        var jobs = await _jobPostService.GetCompanyJobPostsAsync(requesterId);
        return View(jobs);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var job = await _jobPostService.GetDetailsAsync(id);
        return View(job);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateJobPostVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var requesterId = GetUserId();
        await _jobPostService.CreateAsync(requesterId, model.ToCommand());

        return RedirectToAction(nameof(MyJobs));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var job = await _jobPostService.GetDetailsAsync(id);
        if (job is null)
            return NotFound();

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditJobPostVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var requesterId = GetUserId();
        await _jobPostService.UpdateAsync(requesterId, model.Id, model.ToCommand());

        return RedirectToAction(nameof(MyJobs));
    }

    [HttpPost]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var requesterId = GetUserId();
        await _jobPostService.DeactivateAsync(requesterId, id);

        return RedirectToAction(nameof(MyJobs));
    }

    [HttpGet]
    public async Task<IActionResult> Search(JobPostSearchRequestDto dto)
    {
        var jobs = await _jobPostService.SearchAsync(dto);
        return View("Index", jobs);
    }

    [HttpGet]
    public async Task<IActionResult> Filter(JobPostFilterRequestDto dto)
    {
        var jobs = await _jobPostService.FilterAsync(dto);
        return View("Index", jobs);
    }

    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }   }