using System.Security.Claims;
using JobFlowProject.Business.Dto.Feature;
using JobFlowProject.Business.Services.CompaneisService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto.Authentication;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]

public class FeatureController : ControllerBase
{
    
    private readonly FeatureService _featureService;

    public FeatureController(FeatureService featureService)
    {
        _featureService = featureService;
    }
  
    [HttpGet("features")]
    public async Task<IActionResult> GetFeatures()
    {
        var result = await _featureService.GetFeaturesAsync();

        return Ok(new ApiResponse<List<FeatureListDto>>(
            true,
            result,
            "Operation completed successfully."));
    }
    [HttpPost("features")]
    public async Task<IActionResult> CreateFeature(CreateFeatureDto dto)
    {
        await _featureService.CreateFeatureAsync(dto);

        return Ok(new ApiResponse<object>(
            true,
            null,
            "Feature created successfully."));
    }
    [HttpPut("features/{id:guid}")]
    public async Task<IActionResult> UpdateFeature(Guid id, UpdateFeatureDto dto)
    {
        await _featureService.UpdateFeatureAsync(id, dto);

        return Ok(new ApiResponse<object>(
            true,
            null,
            "Feature updated successfully."));
    }
    
    [HttpDelete("features/{id:guid}")]
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