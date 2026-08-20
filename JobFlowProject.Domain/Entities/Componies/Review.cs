using JobFlowProject.Domain.Entites;
using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Domain.Entities.Componies;


public class Review : BaseEntity
{
    private Review() { }
    
    public Review(Guid jobSeekerId, Guid companyId, int rating, string comment, bool isPublic = true)
    {
        JobSeekerId = jobSeekerId;
        CompanyId = companyId;
        Rating = rating;
        Comment = comment;
        IsPublic = isPublic;
        Status = ReviewStatusEnum.Pending;

        Validate();
    }
    
    /// <summary>
    /// آیدی کارجو (نظر‌دهنده)
    /// </summary>
    public Guid JobSeekerId { get; private set; }

    /// <summary>
    /// کارجو
    /// </summary>
    public AppUser JobSeeker { get; private set; }

    /// <summary>
    /// آیدی شرکت
    /// </summary>
    public Guid CompanyId { get; private set; }

    /// <summary>
    /// شرکت
    /// </summary>
    public Company Company { get; private set; }

    /// <summary>
    /// امتیاز از ۱ تا ۵
    /// </summary>
    public int Rating { get; private set; }

    /// <summary>
    /// متن نظر
    /// </summary>
    public string Comment { get; private set; }

    /// <summary>
    /// آیا نظر برای عموم قابل نمایش است؟
    /// </summary>
    public bool IsPublic { get; private set; } = true;

    /// <summary>
    /// وضعیت تایید نظر توسط مدیر
    /// </summary>
    public ReviewStatusEnum Status { get; private set; } = ReviewStatusEnum.Pending;

    /// <summary>
    /// آیا این نظر توسط کاربر گزارش شده است؟
    /// </summary>
    public bool IsReported { get; private set; }

    /// <summary>
    /// دلیل گزارش
    /// </summary>
    public string? ReportReason { get; private set; }

    public void Approve()
    {
        Status = ReviewStatusEnum.Approved;
    }

    public void Reject()
    {
        Status = ReviewStatusEnum.Rejected;
    }

    public void Report(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new Exception("Report reason cannot be empty");

        if (reason.Length > 500)
            throw new Exception("Report reason cannot exceed 500 characters");

        IsReported = true;
        ReportReason = reason;
    }

    public void Hide()
    {
        IsPublic = false;
    }

    public override void Validate()
    {
        if (Rating < 1 || Rating > 5)
            throw new Exception("Rating must be between 1 and 5");

        if (string.IsNullOrWhiteSpace(Comment))
            throw new Exception("Comment cannot be empty");

        if (Comment.Length > 2000)
            throw new Exception("Comment cannot exceed 2000 characters");

        if (JobSeekerId == Guid.Empty)
            throw new Exception("JobSeekerId is required");

        if (CompanyId == Guid.Empty)
            throw new Exception("CompanyId is required");

        if (!Enum.IsDefined(typeof(ReviewStatusEnum), Status))
            throw new Exception("ReviewStatus is invalid");
    }
}
