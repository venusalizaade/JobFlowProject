using JobFlowProject.Domain.Entites.Job;

namespace JobFlowProject.Domain.Entites.User;

/// <summary>
/// ذخیره آگهی توسط کارجو
/// </summary>
public class SavedJob : BaseEntity
{
    private SavedJob() { }

    public SavedJob(Guid appUserId, Guid jobPostId)
    {
        AppUserId = appUserId;
        JobPostId = jobPostId;
    }
    public Guid AppUserId { get;private set; }
    public AppUser AppUser { get; private set; }

    public Guid JobPostId { get; private set; }
    public JobPost JobPost { get; private set; }

    public override void Validation()
    {
        if( AppUserId == Guid.Empty)
            throw new Exception("Profile is required");

        if (JobPostId == Guid.Empty)
            throw new Exception("JobPost is required");
    }
}