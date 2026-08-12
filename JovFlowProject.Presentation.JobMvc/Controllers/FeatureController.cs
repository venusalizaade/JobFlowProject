using JobFlowProject.Business.Interfaces;
using JovFlowProject.JobMvc.Models.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JovFlowProject.JobMvc.Controllers;

[Authorize(Policy = "Admin")]
public class FeatureController : Controller
{
    private readonly IFeatureService _featureService;

    public FeatureController(IFeatureService featureService)
    {
        _featureService = featureService;
    }

    public async Task<IActionResult> Index()
    {
        var features = await _featureService.GetFeaturesAsync();
        return View(features);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateFeatureVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _featureService.CreateFeatureAsync(model.ToDto());
        TempData["Success"] = "فیچر جدید با موفقیت ایجاد شد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var feature = (await _featureService.GetFeaturesAsync()).FirstOrDefault(x => x.Id == id);

        if (feature is null)
        {
            TempData["Error"] = "فیچر موردنظر یافت نشد.";
            return RedirectToAction(nameof(Index));
        }

        return View(new EditFeatureVm
        {
            Id = feature.Id,
            Name = feature.Name,
            Price = feature.Price,
            DurationDays = feature.DurationDays,
            FeatureType = feature.FeatureType
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditFeatureVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _featureService.UpdateFeatureAsync(model.Id, model.ToDto());
        TempData["Success"] = "تغییرات فیچر ذخیره شد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var requesterId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

        await _featureService.DeleteFeatureAsync(id, requesterId);
        TempData["Success"] = "فیچر حذف شد.";

        return RedirectToAction(nameof(Index));
    }
}