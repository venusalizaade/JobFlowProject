using JobFlowProject.Domain.Entites.Logs;
using JobFlowProject.Domain.Entities.Componies;

namespace JobFlowProject.Business.Interfaces.Log;

public interface INotificationService
{
    Task NotifyAdminForEmployerVerificationAsync(Company company);
    Task<List<NotificationLog>> GetUserNotificationsAsync(Guid userId);
    Task<int> GetUnreadCountAsync(Guid userId);
    Task MarkAllAsReadAsync(Guid userId);
}
