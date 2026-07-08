using JobFlowProject.Domain.Entites.Job;

namespace JobFlowProject.Domain.Entites.User;


public class Review : BaseEntity
{
    /// <summary>
    /// آیدی کارجو (نظر‌دهنده)
    /// </summary>
    public Guid JobSeekerId { get; set; }

    /// <summary>
    /// کارجو
    /// </summary>
    public Profile JobSeeker { get; set; }

    /// <summary>
    /// آیدی شرکت
    /// </summary>
    public Guid CompanyId { get; set; }

    /// <summary>
    /// شرکت
    /// </summary>
    public Company Company { get; set; }

    /// <summary>
    /// امتیاز از ۱ تا ۵
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// متن نظر
    /// </summary>
    public string Comment { get; set; }

    /// <summary>
    /// آیا نظر برای عموم قابل نمایش است؟
    /// </summary>
    public bool IsPublic { get; set; } = true;

    public override void Validation()
    {
        if (Rating < 1 || Rating > 5)
            throw new Exception("Rating must be between 1 and 5");

        if (string.IsNullOrWhiteSpace(Comment))
            throw new Exception("Comment is required");
    }
}