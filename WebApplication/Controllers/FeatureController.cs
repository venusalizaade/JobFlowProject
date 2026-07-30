using System.Security.Claims;
using JobFlowProject.Business.Dto.Feature;
using JobFlowProject.Business.Interfaces;
using JobFlowProject.Business.Services.CompaneisService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto.Authentication;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/features")]
[Authorize(Roles = "Admin")]
public class FeatureController : ControllerBase
{
    private readonly IFeatureService _featureService;

    public FeatureController(IFeatureService featureService)
    {
        _featureService = featureService;
    }

    [HttpGet]
    public async Task<IActionResult> GetFeatures()
    {
        var result = await _featureService.GetFeaturesAsync();

        return Ok(new ApiResponse<List<FeatureListDto>>(
            true,
            result,
            "Operation completed successfully."));
    }

    [HttpPost]
    public async Task<IActionResult> CreateFeature(CreateFeatureDto dto)
    {
        await _featureService.CreateFeatureAsync(dto);

        return Ok(new ApiResponse<object>(
            true,
            null,
            "Feature created successfully."));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateFeature(Guid id, UpdateFeatureDto dto)
    {
        await _featureService.UpdateFeatureAsync(id, dto);

        return Ok(new ApiResponse<object>(
            true,
            null,
            "Feature updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteFeature(Guid id)
    {
        var requesterId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _featureService.DeleteFeatureAsync(id, requesterId);

        return Ok(new ApiResponse<object>(
            true,
            null,
            "Feature deleted successfully."));
    }
}