using System.Security.Claims;
using JobFlowProject.Business.Dto.CompanyDto;
using JobFlowProject.Business.Interfaces.EmployerInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/company")]
[Authorize(Roles = "Employer")]
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

        return Ok(result);
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

        return NoContent();
    }
}