using JobFlowProject.Domain.Entites.Logs;

namespace JobFlowProject.Domain.Interfaces.Repository;

public interface INotificationRepository
{
    Task AddAsync(NotificationLog notification);
}