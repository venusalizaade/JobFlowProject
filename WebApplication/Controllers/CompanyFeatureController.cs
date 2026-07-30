using System.Security.Claims;
using JobFlowProject.Business.Dto.Feature;
using JobFlowProject.Business.Interfaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto.Authentication;

namespace WebApplication1.Controllers;



[ApiController]
[Route("api/company-features")]
[Authorize(Roles = "Admin")]
public class CompanyFeatureController : ControllerBase
{
    private readonly ICompanyFeatureService _companyFeatureService;

    public CompanyFeatureController(ICompanyFeatureService companyFeatureService)
    {
        _companyFeatureService = companyFeatureService;
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignFeatureToCompany(
        AssignFeatureToCompanyDto dto)
    {
        await _companyFeatureService.AssignFeatureToCompanyAsync(dto);

        return Ok(new ApiResponse<object>(
            true,
            null,
            "Feature assigned successfully."));
    }

    [HttpDelete("{companyFeatureId:guid}")]
    public async Task<IActionResult> RemoveFeatureFromCompany(Guid companyFeatureId)
    {
        var requesterId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _companyFeatureService.RemoveFeatureFromCompanyAsync(
            companyFeatureId,
            requesterId);

        return Ok(new ApiResponse<object>(
            true,
            null,
            "Feature removed successfully."));
    }

    [HttpGet("company/{companyId:guid}")]
    public async Task<IActionResult> GetCompanyFeatures(Guid companyId)
    {
        var result = await _companyFeatureService.GetCompanyFeaturesAsync(companyId);

        return Ok(new ApiResponse<List<FeatureListDto>>(
            true,
            result,
            "Operation completed successfully."));
    }
   

}
