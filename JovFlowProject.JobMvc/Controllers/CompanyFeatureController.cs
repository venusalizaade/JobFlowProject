using System.Security.Claims;
using JobFlowProject.Business.Interfaces.User;
using JovFlowProject.JobMvc.Models.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JovFlowProject.JobMvc.Controllers;

[Authorize(Policy = "Admin")]
public class CompanyFeatureController : Controller
{
    private readonly ICompanyFeatureService _companyFeatureService;

    public CompanyFeatureController(ICompanyFeatureService companyFeatureService)
    {
        _companyFeatureService = companyFeatureService;
    }

    public async Task<IActionResult> Index(Guid companyId)
    {
        var features = await _companyFeatureService.GetCompanyFeaturesAsync(companyId);

        ViewBag.CompanyId = companyId;

        return View(features);
    }

    [HttpGet]
    public IActionResult Assign(Guid companyId)
    {
        return View(new AssignFeatureToCompanyVm
        {
            CompanyId = companyId
        });
    }

    [HttpPost]
    public async Task<IActionResult> Assign(AssignFeatureToCompanyVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _companyFeatureService.AssignFeatureToCompanyAsync(model.ToDto());

        return RedirectToAction(nameof(Index), new
        {
            companyId = model.CompanyId
        });
    }

    [HttpPost]
    public async Task<IActionResult> Remove(Guid companyFeatureId, Guid companyId)
    {
        var requesterId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await _companyFeatureService.RemoveFeatureFromCompanyAsync(companyFeatureId, requesterId);

        return RedirectToAction(nameof(Index), new
        {
            companyId
        });
    }
}