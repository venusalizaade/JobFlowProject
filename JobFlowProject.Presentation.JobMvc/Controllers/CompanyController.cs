using System.Security.Claims;
using JobFlowProject.Business.Dto.CompanyDto;
using JobFlowProject.Business.Interfaces.EmployerInterfaces;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Enums;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using JovFlowProject.JobMvc.Models.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JovFlowProject.JobMvc.Controllers;

public class CompanyController : Controller
{
    private readonly ICompanyService _companyService;
    private readonly JobFlowDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;

    public CompanyController(
        ICompanyService companyService,
        JobFlowDbContext dbContext,
        UserManager<AppUser> userManager)
    {
        _companyService = companyService;
        _dbContext = dbContext;
        _userManager = userManager;
    }

    [Authorize(Policy = "ApprovedEmployer")]
    public async Task<IActionResult> Details(Guid companyId)
    {
        var requesterId = GetUserId();

        CompanyResponseDto company;
        try
        {
            company = await _companyService.GetCompanyInfoAsync(companyId, requesterId);
        }
        catch (JobFlowProject.Business.Exceptions.BaseExeption.BaseBusinessException)
        {
            return NotFound();
        }

        ViewBag.CityName = await _dbContext.Cities.Where(c => c.Id == company.CityId).Select(c => c.Name).FirstOrDefaultAsync();
        ViewBag.ProvinceName = await _dbContext.provinces.Where(p => p.Id == company.ProvinceId).Select(p => p.Name).FirstOrDefaultAsync();
        ViewBag.LogoUrl = await GetLogoUrlAsync(companyId);

        return View(company);
    }

    [HttpGet]
    [Authorize(Policy = "ApprovedEmployer")]
    public async Task<IActionResult> Edit(Guid companyId)
    {
        var requesterId = GetUserId();

        CompanyResponseDto company;
        try
        {
            company = await _companyService.GetCompanyInfoAsync(companyId, requesterId);
        }
        catch (JobFlowProject.Business.Exceptions.BaseExeption.BaseBusinessException)
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(requesterId.ToString());

        var address = await _dbContext.Companies
            .Where(c => c.Id == companyId)
            .Select(c => c.Address)
            .FirstOrDefaultAsync();

        var model = new EditCompanyVm
        {
            CompanyId = company.CompanyId,
            Name = company.Name,
            ProvinceId = company.ProvinceId,
            CityId = company.CityId,
            Address = address ?? "",
            About = company.About,
            Email = user?.Email ?? "",
            PhoneNumber = user?.PhoneNumber ?? ""
        };

        await PopulateLocationsAsync(model.ProvinceId);
        ViewBag.LogoUrl = await GetLogoUrlAsync(companyId);

        return View(model);
    }

    [HttpPost]
    [Authorize(Policy = "ApprovedEmployer")]
    public async Task<IActionResult> Edit(Guid companyId, EditCompanyVm model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateLocationsAsync(model.ProvinceId);
            ViewBag.LogoUrl = await GetLogoUrlAsync(companyId);
            return View(model);
        }

        var requesterId = GetUserId();

        try
        {
            await _companyService.UpdateByEmployerAsync(companyId, requesterId, model.ToDto());

            var user = await _userManager.FindByIdAsync(requesterId.ToString());
            if (user is not null)
            {
                user.Email = model.Email.Trim();
                user.PhoneNumber = model.PhoneNumber.Trim();
                await _userManager.UpdateAsync(user);
            }

            TempData["Success"] = "اطلاعات شرکت با موفقیت به‌روزرسانی شد.";
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException)
        {
            TempData["Error"] = "ذخیره‌سازی با مشکل مواجه شد؛ لطفاً استان و شهر معتبر انتخاب کنید.";
        }
        catch (JobFlowProject.Business.Exceptions.BaseExeption.BaseBusinessException)
        {
            TempData["Error"] = "این شرکت یافت نشد یا به شما تعلق ندارد.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Details), new { companyId });
    }

    [HttpPost]
    [Authorize(Policy = "ApprovedEmployer")]
    public async Task<IActionResult> UploadLogo(IFormFile file)
    {
        var requesterId = GetUserId();
        var company = await _dbContext.Companies
            .FirstOrDefaultAsync(c => c.AppUserId == requesterId && !c.IsDeleted);

        if (company is null)
        {
            TempData["Error"] = "شرکتی برای این حساب یافت نشد.";
            return RedirectToAction("Dashboard", "Employer");
        }

        try
        {
            await _companyService.UploadLogoAsync(requesterId, file);
            TempData["Success"] = "لوگوی شرکت با موفقیت بارگذاری شد.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Details), new { companyId = company.Id });
    }

    private async Task PopulateLocationsAsync(Guid? selectedProvince = null)
    {
        ViewBag.Provinces = new SelectList(
            await _dbContext.provinces.OrderBy(x => x.Name).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = x.Id == selectedProvince
            }).ToListAsync(), "Value", "Text");

        if (selectedProvince.HasValue)
        {
            ViewBag.Cities = new SelectList(
                await _dbContext.Cities.Where(c => c.ProvinceId == selectedProvince.Value).OrderBy(x => x.Name).Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync(), "Value", "Text");
        }
    }

    private async Task<string?> GetLogoUrlAsync(Guid companyId)
    {
        var fileName = await _dbContext.AttachmentsFiles
            .Where(a => a.CompanyId == companyId && a.AttachmentType == AttachmentType.CompanyLogo && !a.IsDeleted)
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => a.FileName)
            .FirstOrDefaultAsync();

        return fileName is null ? null : "/images/logos/" + fileName;
    }

    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}
