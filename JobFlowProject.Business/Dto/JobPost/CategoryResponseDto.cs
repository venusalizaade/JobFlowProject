namespace JobFlowProject.Business.Dto.CompanyDto;

public record CategoryResponseDto(
    Guid Id,
    string Name,
    string? Description
);
public record  CreateCategoryDto(
   
    string Name,
    string? Description
);

