using JobFlowProject.Domain.Entites.Logs;
using JobFlowProject.Domain.Entities.Componies;
using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Business.Interfaces.Log;

public interface INotificationService
{
    Task NotifyAdminForEmployerVerificationAsync(Company company);
    Task NotifyAsync(Guid userId, string title, string message, NotificationTypeEnum type, Guid? companyId = null, Guid? referenceId = null);
    Task NotifyAdminAsync(string title, string message, NotificationTypeEnum type, Guid? companyId = null, Guid? referenceId = null);
    Task<List<NotificationLog>> GetUserNotificationsAsync(Guid userId, NotificationTypeEnum? type = null);
    Task<int> GetUnreadCountAsync(Guid userId);
    Task MarkAllAsReadAsync(Guid userId);
    Task MarkAsReadAsync(Guid id, Guid userId);
    Task DeleteAsync(Guid id, Guid userId);
}
