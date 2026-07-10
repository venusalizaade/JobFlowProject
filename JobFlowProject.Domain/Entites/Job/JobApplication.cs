using JobFlowProject.Domain.Entites.Resume;
using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Domain.Entites.Job;


public class JobApplication : BaseEntity
{
    /// <summary>
    /// وضعیت درخواست
    /// </summary>
    public ApplicationStatusEnum Status { get; set; } = ApplicationStatusEnum.Pending;

    /// <summary>
    ///آیدی آگهی
    /// </summary>
    public Guid JobPostId { get; set; }

    /// <summary>
    /// آگهی
    /// </summary>
    public JobPost JobPost { get; set; }

    /// <summary>
    /// آیدی کارجو
    /// </summary>
    public Guid JobSeekerId { get; set; }

    /// <summary>
    /// کارجو
    /// </summary>
    public AppUser JobSeeker { get; set; }

    /// <summary>
    ///آیدی فایل رزومه (Attachment)
    /// </summary>
    public Guid AttachmentId { get; set; }

    /// <summary>
    /// فایل رزومه
    /// </summary>
    public AttachmentFile Attachment { get; set; }

    public override void Validation()
    {
        if (JobPostId == Guid.Empty)
            throw new Exception("JobPost is required");

        if (JobSeekerId == Guid.Empty)
            throw new Exception("JobSeeker is required");

        if (AttachmentId == Guid.Empty)
            throw new Exception("Attachment is required");
    }
}