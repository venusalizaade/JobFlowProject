using System.Security.Claims;
using JobFlowProject.Business.Interfaces.JobPost;
using JovFlowProject.JobMvc.Models.Job;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JovFlowProject.JobMvc.Controllers;

[Authorize]
public class JobApplicationController : Controller
{
    private readonly IJobApplicationService _jobApplicationService;

    public JobApplicationController(IJobApplicationService jobApplicationService)
    {
        _jobApplicationService = jobApplicationService;
    }

    [HttpGet]
    public async Task<IActionResult> MyApplications()
    {
        var requesterId = GetUserId();
        var applications = await _jobApplicationService.GetMyApplicationsAsync(requesterId);
        return View(applications);
    }

    [HttpGet]
    public IActionResult Apply(Guid jobPostId)
    {
        return View(new CreateJobApplicationVm
        {
            JobPostId = jobPostId
        });
    }

    [HttpPost]
    public async Task<IActionResult> Apply(CreateJobApplicationVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var requesterId = GetUserId();
        await _jobApplicationService.ApplyAsync(requesterId, model.ToCommand());

        return RedirectToAction(nameof(MyApplications));
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var requesterId = GetUserId();
        var application = await _jobApplicationService.GetDetailsAsync(requesterId, id);
        return View(application);
    }

    [HttpPost]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var requesterId = GetUserId();
        await _jobApplicationService.CancelAsync(requesterId, id);

        return RedirectToAction(nameof(MyApplications));
    }

    [Authorize(Policy = "ApprovedEmployer")]
    [HttpGet]
    public async Task<IActionResult> EmployerApplications(Guid jobPostId)
    {
        var requesterId = GetUserId();
        var applications = await _jobApplicationService.GetJobApplicationsAsync(requesterId, jobPostId);

        ViewBag.JobPostId = jobPostId;
        return View(applications);
    }

    [Authorize(Policy = "ApprovedEmployer")]
    [HttpPost]
    public async Task<IActionResult> ChangeStatus(ChangeApplicationStatusVm model)
    {
        if (!ModelState.IsValid)
            return RedirectToAction(nameof(EmployerApplications), new
            {
                jobPostId = model.JobPostId
            });

        var requesterId = GetUserId();
        await _jobApplicationService.ChangeStatusAsync(requesterId, model.ToCommand());

        return RedirectToAction(nameof(EmployerApplications), new
        {
            jobPostId = model.JobPostId
        });
    }

    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}