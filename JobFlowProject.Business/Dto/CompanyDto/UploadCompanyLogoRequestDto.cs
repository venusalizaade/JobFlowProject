using Microsoft.AspNetCore.Http;

namespace JobFlowProject.Business.Dto.CompanyDto;

public class UploadCompanyLogoRequestDto
{
    public IFormFile File { get; set; } = null!;
}