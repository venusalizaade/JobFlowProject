using JobFlowProject.Domain.Entites.Job;

namespace JobFlowProject.Domain.Entites.User;

/// <summary>
/// ذخیره آگهی توسط کارجو
/// </summary>
public class SavedJob : BaseEntity
{
    public Guid ProfileId { get; set; }
    public Profile Profile { get; set; }

    public Guid JobPostId { get; set; }
    public JobPost JobPost { get; set; }

    public override void Validation()
    {
        if (ProfileId == Guid.Empty)
            throw new Exception("Profile is required");

        if (JobPostId == Guid.Empty)
            throw new Exception("JobPost is required");
    }
}