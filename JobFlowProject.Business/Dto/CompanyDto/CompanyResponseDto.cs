namespace JobFlowProject.Business.Dto.CompanyDto;

public record CompanyResponseDto
(   string Name,
    string NationalId, 
    Guid CityId,
    Guid ProvinceId,
    string? About);