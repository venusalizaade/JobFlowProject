namespace JobFlowProject.Business.Dto.CompanyDto;

public record CompanyResponseDto(
    Guid CompanyId,
    string Name,
    string NationalId,
    Guid CityId,
    Guid ProvinceId,
    string? About);
   
