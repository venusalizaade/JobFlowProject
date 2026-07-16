namespace JobFlowProject.Business.Dto.CompanyDto;

public record UpdateCompanyRequestDto
{
    public string Name { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Province { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string About { get; set; } = null!;
}