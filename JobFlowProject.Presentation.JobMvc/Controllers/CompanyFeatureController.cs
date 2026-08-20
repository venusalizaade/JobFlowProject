using System.Security.Claims;
using JobFlowProject.Business.Exceptions.BaseExeption;
using JobFlowProject.Business.Interfaces.User;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using JovFlowProject.JobMvc.Models.Company;
using JovFlowProject.JobMvc.Models.CompanyFeature;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JovFlowProject.JobMvc.Controllers;

[Authorize(Policy = "Admin")]
public class CompanyFeatureController : Controller
{
    private readonly ICompanyFeatureService _companyFeatureService;
    private readonly JobFlowDbContext _dbContext;

    public CompanyFeatureController(
        ICompanyFeatureService companyFeatureService,
        JobFlowDbContext dbContext)
    {
        _companyFeatureService = companyFeatureService;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Companies()
    {
        var companies = await _dbContext.Companies
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .Select(x => new CompanyFeatureCompaniesVm
            {
                Id = x.Id,
                Name = x.Name,
                NationalId = x.NationalId
            })
            .ToListAsync();

        return View(companies);
    }

    public async Task<IActionResult> Index(Guid companyId)
    {
        var now = DateTime.UtcNow;

        var features = await _dbContext.CompanyFeatures
            .AsNoTracking()
            .Where(x => x.CompanyId == companyId && !x.IsDeleted)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new CompanyFeatureVm
            {
                CompanyFeatureId = x.Id,
                FeatureId = x.FeatureId,
                FeatureName = x.Feature.Name,
                Description = x.Feature.Description,
                FeatureType = x.Feature.FeatureType,
                Price = x.Feature.Price,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                IsActive = x.IsActive,
                DaysRemaining = (int)Math.Ceiling((x.EndDate - now).TotalDays)
            })
            .ToListAsync();

        ViewBag.CompanyId = companyId;
        ViewBag.CompanyName = await _dbContext.Companies
            .Where(x => x.Id == companyId)
            .Select(x => x.Name)
            .FirstOrDefaultAsync();

        return View(features);
    }

    [HttpGet]
    public async Task<IActionResult> Assign(Guid companyId)
    {
        ViewBag.Features = await GetFeatureSelectListAsync();
        ViewBag.CompanyName = await _dbContext.Companies
            .Where(x => x.Id == companyId)
            .Select(x => x.Name)
            .FirstOrDefaultAsync();

        return View(new AssignFeatureToCompanyVm
        {
            CompanyId = companyId
        });
    }

    [HttpPost]
    public async Task<IActionResult> Assign(AssignFeatureToCompanyVm model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Features = await GetFeatureSelectListAsync();
            ViewBag.CompanyName = await _dbContext.Companies
                .Where(x => x.Id == model.CompanyId)
                .Select(x => x.Name)
                .FirstOrDefaultAsync();
            return View(model);
        }

        try
        {
            await _companyFeatureService.AssignFeatureToCompanyAsync(model.ToDto());
            TempData["Success"] = "فیچر با موفقیت به شرکت تخصیص داده شد.";
        }
        catch (ItemNotFoundException ex)
        {
            TempData["Error"] = ex.Message;
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index), new { companyId = model.CompanyId });
    }

    [HttpPost]
    public async Task<IActionResult> Remove(Guid companyFeatureId, Guid companyId)
    {
        var requesterId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await _companyFeatureService.RemoveFeatureFromCompanyAsync(companyFeatureId, requesterId);
        TempData["Success"] = "فیچر از شرکت حذف شد.";

        return RedirectToAction(nameof(Index), new { companyId });
    }

    [HttpGet]
    public async Task<IActionResult> Extend(Guid companyFeatureId, Guid companyId)
    {
        var companyFeature = await _dbContext.CompanyFeatures
            .AsNoTracking()
            .Include(x => x.Feature)
            .FirstOrDefaultAsync(x => x.Id == companyFeatureId && !x.IsDeleted);

        if (companyFeature is null)
        {
            TempData["Error"] = "فیچر موردنظر یافت نشد.";
            return RedirectToAction(nameof(Index), new { companyId });
        }

        return View(new ExtendCompanyFeatureVm
        {
            CompanyFeatureId = companyFeature.Id,
            CompanyId = companyId,
            FeatureName = companyFeature.Feature.Name,
            CurrentEndDate = companyFeature.EndDate,
            NewEndDate = companyFeature.EndDate.AddDays(30)
        });
    }

    [HttpPost]
    public async Task<IActionResult> Extend(ExtendCompanyFeatureVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var requesterId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _companyFeatureService.ExtendCompanyFeatureAsync(model.CompanyFeatureId, model.NewEndDate, requesterId);
            TempData["Success"] = "فیچر با موفقیت تمدید شد.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index), new { companyId = model.CompanyId });
    }

    private async Task<SelectList> GetFeatureSelectListAsync()
    {
        var features = await _dbContext.Features
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name + " — " + x.Price.ToString("N0") + " تومان"
            })
            .ToListAsync();

        return new SelectList(features, "Value", "Text");
    }
}
