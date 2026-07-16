using JobFlowProject.Business.Dto.User;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;

namespace JobFlowProject.Business.Interfaces.User;

public interface IAppUserService
{
    Task<List<AppUserDto>> GetAppUserAsync();
  
}