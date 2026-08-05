namespace WebApplication1.Dto.Authentication;

public record JobPostFilterRequestDto(
    Guid? CategoryId,
    Guid? SkillId,
    decimal? MinSalary,
    decimal? MaxSalary
);
