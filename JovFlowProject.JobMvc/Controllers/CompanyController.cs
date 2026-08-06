using System.Security.Claims;
using JobFlowProject.Business.Interfaces.EmployerInterfaces;
using JovFlowProject.JobMvc.Models.Company;
using Microsoft.AspNetCore.Mvc;

namespace JovFlowProject.JobMvc.Controllers;

public class CompanyController : Controller
{
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    public async Task<IActionResult> Details(Guid companyId)
    {
        var requesterId = GetUserId();
        var company = await _companyService.GetCompanyInfoAsync(companyId, requesterId);
        return View(company);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid companyId)
    {
        var requesterId = GetUserId();
        var company = await _companyService.GetCompanyInfoAsync(companyId, requesterId);

        var model = new EditCompanyVm
        {
            Name = company.Name,
            ProvinceId = company.ProvinceId,
            CityId = company.CityId,
            About = company.About
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid companyId, EditCompanyVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var requesterId = GetUserId();
        await _companyService.UpdateByEmployerAsync(companyId, requesterId, model.ToDto());

        return RedirectToAction(nameof(Details), new { companyId });
    }

    [HttpPost]
    public async Task<IActionResult> UploadLogo(IFormFile file)
    {
        var requesterId = GetUserId();
        await _companyService.UploadLogoAsync(requesterId, file);

        return RedirectToAction(nameof(Details));
    }

    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}