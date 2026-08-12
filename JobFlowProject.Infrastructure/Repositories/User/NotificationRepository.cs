using JobFlowProject.Domain.Entites.Logs;
using JobFlowProject.Domain.Interfaces.Repository;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<NotificationLog>> GetForUserAsync(Guid userId)
    {
        return await _context.NotificationLogs
            .Where(n => n.AppUserId == userId)
            .OrderByDescending(n => n.SentAt)
            .ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync(Guid userId)
    {
        return await _context.NotificationLogs
            .CountAsync(n => n.AppUserId == userId && !n.IsRead);
    }

    public async Task MarkAllAsReadAsync(Guid userId)
    {
        var unread = await _context.NotificationLogs
            .Where(n => n.AppUserId == userId && !n.IsRead)
            .ToListAsync();

        foreach (var item in unread)
            item.IsRead = true;

        await _context.SaveChangesAsync();
    }
}
