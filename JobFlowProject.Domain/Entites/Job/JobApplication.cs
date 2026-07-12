using JobFlowProject.Domain.Entites.Resume;
using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Domain.Entites.Job;


public class JobApplication : BaseEntity
{
    private JobApplication() { }
    
    public JobApplication(Guid jobPostId, Guid jobSeekerId, Guid attachmentId)
    {
       
        
        JobPostId = jobPostId;
        JobSeekerId = jobSeekerId;
        AttachmentId = attachmentId;
        Status = ApplicationStatusEnum.Pending;
    }
    
    /// <summary>
    /// وضعیت درخواست
    /// </summary>
    public ApplicationStatusEnum Status { get;  private set; }

    /// <summary>
    ///آیدی آگهی
    /// </summary>
    public Guid JobPostId { get;  private set; }

    /// <summary>
    /// آگهی
    /// </summary>
    public JobPost JobPost { get;  private set; }

    /// <summary>
    /// آیدی کارجو
    /// </summary>
    public Guid JobSeekerId { get;  private set; }

    /// <summary>
    /// کارجو
    /// </summary>
    public AppUser JobSeeker { get;  private set; }

    /// <summary>
    ///آیدی فایل رزومه (Attachment)
    /// </summary>
    public Guid AttachmentId { get;  private set; }

    /// <summary>
    /// فایل رزومه
    /// </summary>
    public AttachmentFile Attachment { get;  private set; }

    public override void Validate()
    {
        if (JobPostId == Guid.Empty)
            throw new Exception("JobPost is required");

        if (JobSeekerId == Guid.Empty)
            throw new Exception("JobSeeker is required");

        if (AttachmentId == Guid.Empty)
            throw new Exception("Attachment is required");
    }
}