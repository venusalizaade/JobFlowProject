namespace JobFlowProject.Business.Dto.Admin;

public record DashboardDto(
    int TotalUsers,
    int TotalEmployers,
    int TotalJobSeekers,
    int TotalCompanies,
    int TotalJobPosts,
    int PendingEmployers,
    int PendingApplications
);