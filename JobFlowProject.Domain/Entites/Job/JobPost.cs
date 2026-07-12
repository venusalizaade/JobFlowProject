using JobFlowProject.Domain.Entites.Componyes;
using JobFlowProject.Domain.Entites.Componyes.ComponyFeatures;
using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Domain.Entites.Job;

public class JobPost : BaseEntity
{
    private JobPost() { }

    public JobPost(string title, string aboutJob,string city,string province, EmploymentTypeEnum employmentType,
        Guid companyId, Guid employerId, Guid categoryId)
    {
        Title = title;
        AboutJob = aboutJob;
        City = city;
        Province = province;
        EmploymentType = employmentType;
        CompanyId = companyId;
        CategoryId = categoryId;
        IsActive = true;
    }
   
    /// <summary>
    /// عنوان شغل
    /// </summary>
    public string Title { get;  private set; }

    /// <summary>
    /// توضیحات کامل شغل
    /// </summary>
    public string AboutJob { get;  private set; }
    public string City { get;  private set; }
    public string Province { get;  private set; }

    /// <summary>
    /// حقوق (اختیاری)
    /// </summary>
    public string? Salary { get; set; }

    /// <summary>
    /// نوع همکاری
    /// </summary>
    public EmploymentTypeEnum EmploymentType { get;  private set; }

    /// <summary>
    /// وضعیت فعال بودن آگهی
    /// </summary>
    public bool IsActive { get;  private set; } = true;

    /// <summary>
    /// تاریخ انقضای آگهی
    /// </summary>
    public DateTime ExpiresAt { get;  private set; }

    /// <summary>
    /// آیدی شرکت
    /// </summary>
    public Guid CompanyId { get;  private set; }

    /// <summary>
    /// شرکت
    /// </summary>
    public Company Company { get;  private set; }


    /// <summary>
    /// آیدی دسته‌بندی
    /// </summary>
    public Guid CategoryId { get;  private set; }

    /// <summary>
    /// دسته‌بندی
    /// </summary>
    public Category Category { get;  private set; }

    /// <summary>
    /// لیست درخواست‌های ارسال‌شده
    /// </summary>
    public ICollection<JobApplication> JobApplications { get;  private set; } = new List<JobApplication>();
    
    /// <summary>
    /// لیست پرداخت‌های شرکت
    /// </summary>
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();


    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Title))
            throw new Exception("Title cannot be empty");

        if (Title.Length < 3)
            throw new Exception("Title must be at least 3 characters");

        if (Title.Length > 200)
            throw new Exception("Title cannot exceed 200 characters");

        if (string.IsNullOrWhiteSpace(AboutJob))
            throw new Exception("AboutJob cannot be empty");

        if (AboutJob.Length < 10)
            throw new Exception("AboutJob must be at least 10 characters");

        if (ExpiresAt <= CreatedAt)
            throw new Exception("ExpiresAt must be after CreatedAt");
    }
}