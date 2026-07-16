namespace JobFlowProject.Business.Interfaces.User;

public interface IAdminService
{
    Task VerifyEmployerAsync(Guid employerId);
}