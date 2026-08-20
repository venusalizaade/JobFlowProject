namespace JobFlowProject.Business.Dto.JobPost;

public record JobPostListDto(
    Guid Id,
    string Title,
    string CompanyName,
    string CategoryName,
    string CityName,
    decimal? Salary,
    bool IsActive,
    IReadOnlyList<string> FeatureNames
);
