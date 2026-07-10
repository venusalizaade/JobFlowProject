

using JobFlowProject.Domain.Entites.Componyes;
using JobFlowProject.Domain.Entites.Job;
using JobFlowProject.Domain.Entites.Logs;
using JobFlowProject.Domain.Entites.Resume;
using JobFlowProject.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace JobFlowProject.Domain.Entites.User;


public class AppUser : IdentityUser<Guid> , IEntity
{
    //tTodo اسم کلاس عوض شود
    //// <summary>
    /// نام
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// نام خانوادگی
    /// </summary>
    public string LastName { get; set; }
    
    public string Gender { get; set; }

    /// <summary>
    ///توضیحات
    /// </summary>
    public int? About{ get; set; }
    

    /// <summary>
    /// تاریخ ایجاد
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// وضعیت حذف 
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// تاریخ حذف 
    /// </summary>
    public DateTime DeletedAt { get; set; }
    

    /// <summary>
    /// تاریخ بروزرسانی
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// آیدی شرکت (در صورت کارفرما بودن)
    /// </summary>
    public Guid? CompanyId { get; set; }

    /// <summary>
    /// شرکت مرتبط
    /// </summary>
    
    public Company? Company { get; set; }

    /// <summary>
    /// لیست رزومه‌های کاربر (در صورتی که کارجو باشد)
    /// </summary>
    public ICollection<AttachmentFile> Attachments { get; set; } = new List<AttachmentFile>();
    

    /// <summary>
    /// لیست درخواست‌های شغلی (در صورت کارجو بودن)
    /// </summary>
    public ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();

    /// <summary>
    /// لیست آگهی‌های ذخیره شده
    /// </summary>
    public ICollection<SavedJob> SavedJobs { get; set; } = new List<SavedJob>();

    /// <summary>
    /// لیست نوتیفیکیشن‌های کاربر
    /// </summary>
    public ICollection<NotificationLog> Notifications { get; set; } = new List<NotificationLog>();
    
    }