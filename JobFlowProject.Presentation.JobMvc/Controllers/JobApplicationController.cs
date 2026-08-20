using System.Security.Claims;
using JobFlowProject.Business.Interfaces.JobPost;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using JobFlowProject.Infrastructure.Repositories.User;
using JovFlowProject.JobMvc.Models.Job;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JovFlowProject.JobMvc.Controllers;

[Authorize]
public class JobApplicationController : Controller
{
    private readonly IJobApplicationService _jobApplicationService;
    private readonly IAttachmentRepository _attachmentRepository;
    private readonly JobFlowDbContext _dbContext;

    public JobApplicationController(
        IJobApplicationService jobApplicationService,
        IAttachmentRepository attachmentRepository,
        JobFlowDbContext dbContext)
    {
        _jobApplicationService = jobApplicationService;
        _attachmentRepository = attachmentRepository;
        _dbContext = dbContext;
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

        try
        {
            await _jobApplicationService.ApplyAsync(requesterId, model.ToCommand());
            return RedirectToAction(nameof(MyApplications));
        }
        catch (Exception e)
        {
            TempData["Error"] = e.Message;
            return RedirectToAction("Details", "JobPost", new { id = model.JobPostId });
        }
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
        try
        {
            await _jobApplicationService.ChangeStatusAsync(requesterId, model.ToCommand());
        }
        catch (JobFlowProject.Business.Exceptions.BaseExeption.StatusChangeNotAllowedException)
        {
            TempData["Error"] = "امکان برگرداندن وضعیت از مصاحبه، قبول یا رد به مرحله قبل وجود ندارد.";
        }

        return RedirectToAction(nameof(EmployerApplications), new
        {
            jobPostId = model.JobPostId
        });
    }

    [Authorize(Policy = "ApprovedEmployer")]
    [HttpGet]
    public async Task<IActionResult> DownloadApplicantResume(Guid applicantId)
    {
        var requesterId = GetUserId();

        var hasAccess = await _dbContext.JobApplications.AnyAsync(a =>
            a.JobSeekerId == applicantId &&
            a.JobPost.Company.AppUserId == requesterId);

        if (!hasAccess)
            return NotFound();

        var resume = await _attachmentRepository.GetByUserIdAsync(applicantId);
        if (resume is null || !System.IO.File.Exists(resume.FilePath))
            return NotFound();

        return PhysicalFile(resume.FilePath, resume.FileType ?? "application/octet-stream", resume.FileName);
    }

    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}