


using JobFlowProject.Domain.Entites.Logs;
using JobFlowProject.Domain.Entites.Resume;
using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities.Componies;
using JobFlowProject.Domain.Entities.Job;
using JobFlowProject.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace JobFlowProject.Domain.Entities.User;


public class 
    AppUser : IdentityUser<Guid> , IEntity
{
    
    private AppUser()
    {
    }
    public AppUser(string firstName, string lastName, string nationalId,string email,string phoneNumber ,
        string gender, Guid? requesterId = null)
    {
        FirstName = firstName;
        LastName = lastName;
        UserName = nationalId;
        NationalId = nationalId;
        Email=email;
        PhoneNumber=phoneNumber;
        Gender = gender;
        Validate();
    }

   

  


    //tTodo اسم کلاس عوض شود
    //// <summary>
    /// نام
    /// </summary>
    public string FirstName { get;  set; }

    /// <summary>
    /// نام خانوادگی
    /// </summary>
    public string LastName { get;  set; }
    /// <summary>
    /// جنسیت
    /// </summary>
    public string? Gender { get;  set; }
    /// <summary>
    /// کدملی
    /// </summary>
    public string NationalId { get; private set; }

    /// <summary>
    ///توضیحات
    /// </summary>
    public String? About{ get;  set; }
    
    
    
    /// <summary>
    ///  تائید ثبت نام کاربر توسط ادمین
    /// </summary>
    public bool IsApproved { get;  set; } = false;
    
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
   
 
    

    /// <summary>
    /// شرکت مرتبط
    /// </summary>
    
    public Company? Company { get; private set; }
    public Guid? CompanyId { get; private set; }

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

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(FirstName))
            throw new Exception("FirstName cannot be empty");

        if (FirstName.Length < 2 || FirstName.Length > 100)
            throw new Exception("FirstName must be between 2 and 100 characters");

        if (string.IsNullOrWhiteSpace(LastName))
            throw new Exception("LastName cannot be empty");

        if (LastName.Length < 2 || LastName.Length > 100)
            throw new Exception("LastName must be between 2 and 100 characters");

    }
    public void SetCompany(Guid companyId)
    {
        CompanyId = companyId;
    }
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
}
    