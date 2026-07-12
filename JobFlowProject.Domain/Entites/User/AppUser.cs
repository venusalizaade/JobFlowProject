

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
    ///  تائید ثبت نام کاربر توسط ادمین
    /// </summary>
    public bool IsApproved { get; private set; } = false;
    
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public Guid? CreatedById { get; protected set; }
    public AppUser? Creator { get; protected set; }
    public DateTime? ModifiedAt { get; private set; }
    public Guid? ModifiedById { get; private set; }
    public AppUser? Modifier { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public Guid? DeletedById { get; set; }
    public AppUser? Deleter { get; set; }
    public bool IsDeleted { get; private set; }
   
    public void SetAsDeleted(Guid requesterId)
    {
        DeletedAt = DateTime.UtcNow;
        IsDeleted = true;
        DeletedById = requesterId;
    }

    public void SetModificationInfo(Guid requesterId)
    {
        ModifiedAt = DateTime.UtcNow;
        ModifiedById = requesterId;
    }


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
    
    private void Validation()
    {
        if (string.IsNullOrWhiteSpace(FirstName))
            throw new Exception("FirstName cannot be empty");

        if (FirstName.Length < 2 || FirstName.Length > 100)
            throw new Exception("FirstName must be between 2 and 100 characters");

        if (string.IsNullOrWhiteSpace(LastName))
            throw new Exception("LastName cannot be empty");

        if (LastName.Length < 2 || LastName.Length > 100)
            throw new Exception("LastName must be between 2 and 100 characters");

        if (string.IsNullOrWhiteSpace(Email))
            throw new Exception("Email cannot be empty");

        if (!Email.Contains("@") || !Email.Contains("."))
            throw new Exception("Email is not valid");

        if (!string.IsNullOrWhiteSpace(PhoneNumber) && PhoneNumber.Length != 11)
            throw new Exception("PhoneNumber must be exactly 11 digits");
    }
    }
    