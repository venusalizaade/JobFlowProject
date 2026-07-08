using JobFlowProject.Domain.Entites.User;

namespace JobFlowProject.Domain.Entites.Job;


public class Company : BaseEntity
{
    /// <summary>
    /// نام شرکت
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// شناسه ملی
    /// </summary>
    public string NationalId { get; set; }

    /// <summary>
    /// آدرس
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// درباره شرکت
    /// </summary>
    public string? About { get; set; }
    
    /// <summary>
    /// تعداد آگهی رایگان مجاز
    /// </summary>
    public int FreeJobPostLimit { get; set; } 

    /// <summary>
    /// مدت زمان پیش‌فرض هر آگهی (روز)
    /// </summary>
    public int DefaultJobPostDurationDays { get; set; } 

    /// <summary>
    /// لیست کاربران مرتبط با شرکت
    /// </summary>
    public ICollection<Profile> Profiles { get; set; } = new List<Profile>();

    /// <summary>
    /// لیست آگهی‌های شرکت
    /// </summary>
    public ICollection<JobPost> JobPosts { get; set; } = new List<JobPost>();

    /// <summary>
    /// لیست پرداخت‌های شرکت
    /// </summary>
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    
    /// <summary>
    /// لیست نظراتی که برای این شرکت نوشته شده
    /// </summary>
    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    public override void Validation()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new Exception("Company name is required");
    }
}