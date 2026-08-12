using JobFlowProject.Domain.Entites.Logs;

namespace JobFlowProject.Domain.Interfaces.Repository;

public interface INotificationRepository
{
    Task AddAsync(NotificationLog notification);
    Task<List<NotificationLog>> GetForUserAsync(Guid userId);
    Task<int> GetUnreadCountAsync(Guid userId);
    Task MarkAllAsReadAsync(Guid userId);
}
