using JobFlowProject.Business.Interfaces.Log;
using JobFlowProject.Domain.Entites.Logs;
using JobFlowProject.Domain.Entities.Componies;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Enums;
using JobFlowProject.Domain.Interfaces.Repository;

namespace JobFlowProject.Business.Services.Log;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;

    public NotificationService(INotificationRepository repository)
    {
        _repository = repository;
    }

    public async Task NotifyAdminForEmployerVerificationAsync(
        AppUser user,
        Company company)
    {
        var notification = new NotificationLog(
            "Employer Verification",
            $"{company.Name} registered and needs verification.",
            NotificationTypeEnum.EmployerVerificationRequired,
            user.Id,
            company.Id);

        await _repository.AddAsync(notification);
    }
}