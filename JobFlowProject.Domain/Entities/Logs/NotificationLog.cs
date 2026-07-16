
using JobFlowProject.Domain.Entites.Job;
using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities;
using JobFlowProject.Domain.Entities.Componies;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Domain.Entites.Logs;


public class NotificationLog : BaseEntity
{
    
    public NotificationLog(
        string title,
        string message,
        NotificationTypeEnum type,
        Guid? appUserId = null,
        Guid? companyId = null)
    {
        Title = title;
        Message = message;
        Type = type;
        AppUserId = appUserId;
        CompanyId = companyId;
        SentAt = DateTime.UtcNow;
    }
    /// <summary>
    /// عنوان
    /// </summary>
    public string Title { get;private  set; }

    /// <summary>
    /// متن پیام
    /// </summary>
    public string Message { get; private  set; }

    /// <summary>
    /// نوع نوتیفیکیشن
    /// </summary>
    public NotificationTypeEnum Type { get;private  set; }

    /// <summary>
    /// زمان ارسال
    /// </summary>
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// آیدی کاربر (در صورت وجود)
    /// </summary>
    public Guid? AppUserId { get; set; }

    /// <summary>
    /// کاربر
    /// </summary>
    public AppUser? AppUser { get; set; }

    /// <summary>
    ///آیدی شرکت (در صورت وجود)
    /// </summary>
    public Guid? CompanyId { get; set; }

    /// <summary>
    /// شرکت
    /// </summary>
    public Company? Company { get; set; }

    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Title))
            throw new Exception("Title cannot be empty");

        if (string.IsNullOrWhiteSpace(Message))
            throw new Exception("Message cannot be empty");

        if (Message.Length > 2000)
            throw new Exception("Message cannot exceed 2000 characters");
    }
}