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

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        return View(new EditFeatureVm { Id = id });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditFeatureVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _featureService.UpdateFeatureAsync(model.Id, model.ToDto());

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var requesterId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

        await _featureService.DeleteFeatureAsync(id, requesterId);

        return RedirectToAction(nameof(Index));
    }
}