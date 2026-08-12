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

    public async Task<List<NotificationLog>> GetUserNotificationsAsync(Guid userId)
    {
        return await _repository.GetForUserAsync(userId);
    }

    public async Task<int> GetUnreadCountAsync(Guid userId)
    {
        return await _repository.GetUnreadCountAsync(userId);
    }

    public async Task MarkAllAsReadAsync(Guid userId)
    {
        await _repository.MarkAllAsReadAsync(userId);
    }
}
