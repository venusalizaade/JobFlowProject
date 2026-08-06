namespace JobFlowProject.Business.Dto.CompanyDto;

public record UpdateCompanyRequestDto(string Name, Guid ProvinceId, Guid CityId, string Address, string? About)
{
    public string Name { get; set; } = null!;

    public Guid CityId { get; set; } 

    public Guid ProvinceId { get; set; }

    public string Address { get; set; } = null!;

    public string About { get; set; } = null!;
}