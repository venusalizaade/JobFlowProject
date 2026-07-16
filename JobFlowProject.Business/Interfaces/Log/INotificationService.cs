using JobFlowProject.Domain.Entities.Componies;
using JobFlowProject.Domain.Entities.User;

namespace JobFlowProject.Business.Interfaces.Log;

public interface INotificationService
{
    Task NotifyAdminForEmployerVerificationAsync(AppUser user, Company company);
}