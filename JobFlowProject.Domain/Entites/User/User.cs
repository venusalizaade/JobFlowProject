

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
    public string FirstName { get;  set; }

    /// <summary>
    /// نام خانوادگی
    /// </summary>
    public string LastName { get;  set; }
    
    public string Gender { get; private set; }

    /// <summary>
    ///توضیحات
    /// </summary>
    public int? About{ get; private set; }
    

    /// <summary>
    /// تاریخ ایجاد
    /// </summary>
    public DateTime? CreatedAt { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// وضعیت حذف 
    /// </summary>
    public bool IsDeleted { get; private set; }

    /// <summary>
    /// تاریخ حذف 
    /// </summary>
    public DateTime? DeletedAt { get; private set; }
    

    /// <summary>
    /// تاریخ بروزرسانی
    /// </summary>
    public DateTime UpdatedAt { get; private set; }
    /// <summary>
    ///  تائید ثبت نام کاربر توسط ادمین
    /// </summary>
    public bool IsApprovedByAdmin { get; private set; } = false;

    /// <summary>
    /// آیدی شرکت (در صورت کارفرما بودن)
    /// </summary>
    public Guid? CompanyId { get; private set; }

    /// <summary>
    /// شرکت مرتبط
    /// </summary>
    
    public Company? Company { get; private set; }

    /// <summary>
    /// لیست رزومه‌های کاربر (در صورتی که کارجو باشد)
    /// </summary>
    public ICollection<AttachmentFile> Attachments { get; private set; } = new List<AttachmentFile>();
    

    /// <summary>
    /// لیست درخواست‌های شغلی (در صورت کارجو بودن)
    /// </summary>
    public ICollection<JobApplication> JobApplications { get; private set; } = new List<JobApplication>();

    /// <summary>
    /// لیست آگهی‌های ذخیره شده
    /// </summary>
    public ICollection<SavedJob> SavedJobs { get; private set; } = new List<SavedJob>();

    /// <summary>
    /// لیست نوتیفیکیشن‌های کاربر
    /// </summary>
    public ICollection<NotificationLog> Notifications { get; private set; } = new List<NotificationLog>();
    
    }