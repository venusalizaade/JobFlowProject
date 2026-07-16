using JobFlowProject.Domain.Entites.Logs;
using JobFlowProject.Domain.Interfaces.Repository;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;

namespace JobFlowProject.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly JobFlowDbContext _context;

    public NotificationRepository(JobFlowDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(NotificationLog notification)
    {
        await _context.NotificationLogs.AddAsync(notification);
        await _context.SaveChangesAsync();
    }
}