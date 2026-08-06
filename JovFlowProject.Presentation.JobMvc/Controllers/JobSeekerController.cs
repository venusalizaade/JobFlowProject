using System.Security.Claims;
using JobFlowProject.Business.Interfaces.User;
using JovFlowProject.JobMvc.Models.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JovFlowProject.JobMvc.Controllers;

[Authorize(Policy = "JobSeeker")]
public class JobSeekerController : Controller
{
    private readonly IJobSeekerService _jobSeekerService;

    public JobSeekerController(IJobSeekerService jobSeekerService)
    {
        _jobSeekerService = jobSeekerService;
    }

    public async Task<IActionResult> Profile()
    {
        var requesterId = GetUserId();
        var profile = await _jobSeekerService.GetProfileAsync(requesterId);
        return View(profile);
    }

    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var requesterId = GetUserId();
        var profile = await _jobSeekerService.GetProfileAsync(requesterId);

        var model = new EditJobSeekerProfileVm
        {
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            PhoneNumber = profile.PhoneNumber,
            Gender = profile.Gender,
            About = profile.About
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditJobSeekerProfileVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var requesterId = GetUserId();
        await _jobSeekerService.UpdateProfileAsync(requesterId, model.ToDto());

        return RedirectToAction(nameof(Profile));
    }

    [HttpPost]
    public async Task<IActionResult> UploadResume(IFormFile file)
    {
        var requesterId = GetUserId();
        await _jobSeekerService.UploadResumeAsync(requesterId, file);

        return RedirectToAction(nameof(Profile));
    }

    [HttpPost]
    public async Task<IActionResult> ReplaceResume(IFormFile file)
    {
        var requesterId = GetUserId();
        await _jobSeekerService.ReplaceResumeAsync(requesterId, file);

        return RedirectToAction(nameof(Profile));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteResume()
    {
        var requesterId = GetUserId();
        await _jobSeekerService.DeleteResumeAsync(requesterId);

        return RedirectToAction(nameof(Profile));
    }

    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}