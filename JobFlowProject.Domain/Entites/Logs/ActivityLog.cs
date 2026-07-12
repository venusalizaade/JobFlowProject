using JobFlowProject.Domain.Entites.User;

namespace JobFlowProject.Domain.Entites.Logs;


public class ActivityLog : BaseEntity
{
    /// <summary>
    /// نام عملیات
    /// </summary>
    public string Action { get; set; }

    /// <summary>
    /// جزئیات بیشتر
    /// </summary>
    public string? Details { get; set; }

    /// <summary>
    /// آیدی کاربر انجام‌دهنده
    /// </summary>
    public Guid ProfileId { get; set; }

    /// <summary>
    /// کاربر
    /// </summary>
    public AppUser Profile { get; set; }

    /// <summary>
    ///آیدی موجودیت مرتبط
    /// </summary>
    public Guid? EntityId { get; set; }

    /// <summary>
    /// نوع موجودیت مرتبط
    /// </summary>
    public string? EntityType { get; set; }

    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Action))
            throw new Exception("Action cannot be empty");

        if (Action.Length > 200)
            throw new Exception("Action cannot exceed 200 characters");
    }
}