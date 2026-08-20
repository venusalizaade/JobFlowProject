using JobFlowProject.Domain.Entites.Logs;
using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Domain.Interfaces.Repository;

public interface INotificationRepository
{
    Task AddAsync(NotificationLog notification);
    Task<List<NotificationLog>> GetForUserAsync(Guid userId, NotificationTypeEnum? type = null);
    Task<int> GetUnreadCountAsync(Guid userId);
    Task MarkAllAsReadAsync(Guid userId);
    Task MarkAsReadAsync(Guid id, Guid userId);
    Task DeleteAsync(Guid id, Guid userId);
}
