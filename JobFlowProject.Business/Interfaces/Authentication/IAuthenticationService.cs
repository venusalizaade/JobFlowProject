using JobFlowProject.Business.Dto.Commands;
using JobFlowProject.Business.Dto.Token;

namespace JobFlowProject.Business.Interfaces;

public interface IAuthenticationService
{
    Task <TokenLoginResult> LoginAsync(LoginCommand loginCommand);
    Task <JobSeekerRegisterResult> JobSeekerRegisterAsync(RegisterJobSeekerCommand command);
    Task <EmployerRegisterResult> EmployerRegisterAsync(RegisterEmployerCommand command);

        
    
}