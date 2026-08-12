namespace JobFlowProject.Business.Dto.CompanyDto;

public record UpdateCompanyRequestDto(string Name, Guid ProvinceId, Guid CityId, string Address, string? About);
