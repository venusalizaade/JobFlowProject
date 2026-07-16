using JobFlowProject.Business.Dto.User;
using JobFlowProject.Business.Interfaces.User;
using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Interfaces.Repository;

namespace JobFlowProject.Business.Services.User;

public class AppUserSerivce : IAppUserService
{
    private readonly IUserRepository _userRepository;

    public AppUserSerivce(IUserRepository userRepository)
    {
        userRepository=_userRepository;
    }

    public Task<List<AppUserDto>> GetAppUserAsync()
    {
        throw new NotImplementedException();
    }
}
    
