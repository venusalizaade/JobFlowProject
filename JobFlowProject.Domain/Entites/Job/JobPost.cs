using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Domain.Entites.Job;

public class JobPost : BaseEntity
{
    /// <summary>
    /// عنوان شغل
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// توضیحات کامل شغل
    /// </summary>
    public string AboutJob { get; set; }

    /// <summary>
    /// حقوق (اختیاری)
    /// </summary>
    public string? Salary { get; set; }

    /// <summary>
    /// نوع همکاری
    /// </summary>
    public EmploymentTypeEnum EmploymentType { get; set; }

    /// <summary>
    /// وضعیت فعال بودن آگهی
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// تاریخ انقضای آگهی
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// آیدی شرکت
    /// </summary>
    public Guid CompanyId { get; set; }

    /// <summary>
    /// شرکت
    /// </summary>
    public Company Company { get; set; }

    /// <summary>
    /// آیدی کارفرما (منتشرکننده)
    /// </summary>
    public Guid EmployerId { get; set; }

    /// <summary>
    /// کارفرما
    /// </summary>
    public Profile Employer { get; set; }

    /// <summary>
    /// آیدی دسته‌بندی
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// دسته‌بندی
    /// </summary>
    public Category Category { get; set; }

    /// <summary>
    /// لیست درخواست‌های ارسال‌شده
    /// </summary>
    public ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();

    /// <summary>
    /// لیست کاربرانی که آگهی را ذخیره کرده‌اند
    /// </summary>
    public ICollection<SavedJob> SavedJobs { get; set; } = new List<SavedJob>();

    public override void Validation()
    {
        if (string.IsNullOrWhiteSpace(Title))
            throw new Exception("Title is required");

        if (string.IsNullOrWhiteSpace(AboutJob))
            throw new Exception("AboutJob is required");

        if (ExpiresAt <= CreatedAt)
            throw new Exception("ExpiresAt must be after CreatedAt");
    }
}