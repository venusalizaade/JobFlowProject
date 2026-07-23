using JobFlowProject.Domain.Entities;

namespace JobFlowProject.Domain.Interfaces.Repository.User;

public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
{
    Task<RefreshToken?> GetByTokenAsync(string token);
}