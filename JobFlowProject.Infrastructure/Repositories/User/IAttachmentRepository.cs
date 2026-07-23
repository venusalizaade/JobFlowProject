using JobFlowProject.Domain.Entites.Resume;
using JobFlowProject.Domain.Interfaces.Repository;

namespace JobFlowProject.Infrastructure.Repositories.User;

public interface IAttachmentRepository : IGenericRepository<AttachmentFile>
{
    Task<AttachmentFile?> GetByUserIdAsync(Guid userId);
}