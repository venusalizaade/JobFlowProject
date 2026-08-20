using JobFlowProject.Domain.Entites.Logs;
using JobFlowProject.Domain.Enums;
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

    public async Task<List<NotificationLog>> GetForUserAsync(Guid userId, NotificationTypeEnum? type = null)
    {
        var query = _context.NotificationLogs
            .Where(n => n.AppUserId == userId);

        if (type.HasValue)
            query = query.Where(n => n.Type == type.Value);

        return await query
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

    public async Task MarkAsReadAsync(Guid id, Guid userId)
    {
        var notification = await _context.NotificationLogs
            .FirstOrDefaultAsync(n => n.Id == id && n.AppUserId == userId);

        if (notification is null) return;

        notification.IsRead = true;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        var notification = await _context.NotificationLogs
            .FirstOrDefaultAsync(n => n.Id == id && n.AppUserId == userId);

        if (notification is null) return;

        _context.NotificationLogs.Remove(notification);
        await _context.SaveChangesAsync();
    }
}
