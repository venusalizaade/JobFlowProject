using JobFlowProject.Domain.Entites.Componyes;
using JobFlowProject.Domain.Entites.Job;
using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Domain.Entites.Logs;


public class NotificationLog : BaseEntity
{
    /// <summary>
    /// عنوان
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// متن پیام
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// نوع نوتیفیکیشن
    /// </summary>
    public NotificationTypeEnum Type { get; set; }

    /// <summary>
    /// زمان ارسال
    /// </summary>
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// آیدی کاربر (در صورت وجود)
    /// </summary>
    public Guid? ProfileId { get; set; }

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