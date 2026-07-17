using System.Linq.Expressions;
using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities.User;

namespace JobFlowProject.Domain.Interfaces.Repository;

public interface IUserRepository
{
    Task<AppUser?> GetUserByNationalIdAsync(string nationalId);
   
    
}