using JobFlowProject.Business.Constants;
using JobFlowProject.Business.Exceptions.Authentication_Exceptions;
using JobFlowProject.Business.Interfaces.Log;
using JobFlowProject.Domain.Entites.Logs;
using JobFlowProject.Domain.Entities.Componies;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Enums;
using JobFlowProject.Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Identity;

namespace JobFlowProject.Business.Services.Log;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;
    private readonly UserManager<AppUser> _userManager;

    public NotificationService(
        INotificationRepository repository,
        UserManager<AppUser> userManager)
    {
        _repository = repository;
        _userManager = userManager;
    }

    public async Task NotifyAdminForEmployerVerificationAsync(Company company)
    {
        var admins = await _userManager.GetUsersInRoleAsync(RoleConstants.AdminRoleName);

        var admin = admins.FirstOrDefault();

        if (admin is null)
            throw new UserNotFoundException();

        var notification = new NotificationLog(
            "Employer Verification",
            $"{company.Name} registered and needs verification.",
            NotificationTypeEnum.EmployerVerificationRequired,
            admin.Id,
            company.Id);

        await _repository.AddAsync(notification);
    }

    public async Task NotifyAsync(Guid userId, string title, string message, NotificationTypeEnum type, Guid? companyId = null, Guid? referenceId = null)
    {
        var notification = new NotificationLog(title, message, type, userId, companyId, referenceId);
        await _repository.AddAsync(notification);
    }

    public async Task NotifyAdminAsync(string title, string message, NotificationTypeEnum type, Guid? companyId = null, Guid? referenceId = null)
    {
        var admins = await _userManager.GetUsersInRoleAsync(RoleConstants.AdminRoleName);

        var admin = admins.FirstOrDefault();

        if (admin is null)
            throw new UserNotFoundException();

        var notification = new NotificationLog(title, message, type, admin.Id, companyId, referenceId);
        await _repository.AddAsync(notification);
    }

    public async Task<List<NotificationLog>> GetUserNotificationsAsync(Guid userId, NotificationTypeEnum? type = null)
    {
        return await _repository.GetForUserAsync(userId, type);
    }

    public async Task<int> GetUnreadCountAsync(Guid userId)
    {
        return await _repository.GetUnreadCountAsync(userId);
    }

    public async Task MarkAllAsReadAsync(Guid userId)
    {
        await _repository.MarkAllAsReadAsync(userId);
    }

    public async Task MarkAsReadAsync(Guid id, Guid userId)
    {
        await _repository.MarkAsReadAsync(id, userId);
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        await _repository.DeleteAsync(id, userId);
    }
}
