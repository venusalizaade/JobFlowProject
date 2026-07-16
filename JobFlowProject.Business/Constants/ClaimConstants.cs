using System.Security.Claims;

namespace JobFlowProject.Business.Constants;

public static class ClaimConstants
{
    public static readonly Claim Employer = new(
        ClaimTypes.Role,
        RoleConstants.EmployerRoleName);

    public static readonly Claim JobSeeker = new(
        ClaimTypes.Role,
        RoleConstants.JobSeekerRoleName);

    public static readonly Claim Admin = new(
        ClaimTypes.Role,
        RoleConstants.AdminRoleName);

    public const string UserId = ClaimTypes.NameIdentifier;
}