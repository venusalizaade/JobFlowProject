using System.Security.Claims;
using JobFlowProject.Business.Dto.CompanyDto;
using JobFlowProject.Business.Interfaces.EmployerInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto.Authentication;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/company")]
[Authorize(Policy = "ApprovedEmployer")]
public class CompanyController : ControllerBase

{
    private readonly ICompanyService _companyService;


    public CompanyController(ICompanyService companyService)

    {
        _companyService = companyService;
    }


    [HttpGet("{companyId:guid}")]
    public async Task<IActionResult> GetCompanyInfo(
        [FromRoute] Guid companyId)

    {
        var requesterId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);


        var result = await _companyService.GetCompanyInfoAsync(
            companyId,
            requesterId);


        return Ok(new ApiResponse<object>(
            true,
            result,
            null));
    }


    [HttpPut("{companyId:guid}")]
    public async Task<IActionResult> UpdateCompany(
        [FromRoute] Guid companyId,
        [FromBody] UpdateCompanyRequestDto request)

    {
        var requesterId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);


        await _companyService.UpdateByEmployerAsync(
            companyId,
            requesterId,
            request);


        return Ok(new ApiResponse<object>(
            true,
            null,
            "Operation completed successfully."));
    }
    [HttpPost("logo")]
    public async Task<IActionResult> UploadLogo(
        [FromForm] UploadCompanyLogoRequestDto request)
    {
        var requesterId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _companyService.UploadLogoAsync(
            requesterId,
            request.File);

        return Ok(new ApiResponse<object>(
            true,
            null,
            null));
    }
}