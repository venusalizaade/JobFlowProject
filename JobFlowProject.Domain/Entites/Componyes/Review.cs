using JobFlowProject.Domain.Entites.User;

namespace JobFlowProject.Domain.Entites.Componyes;


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
    }
}