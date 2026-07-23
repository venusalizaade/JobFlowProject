using JobFlowProject.Domain.Entites.Resume;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace JobFlowProject.Infrastructure.Repositories.User;

public class AttachmentRepository
    : GenericRepository<AttachmentFile>,
        IAttachmentRepository
{
    private readonly JobFlowDbContext _context;

    public AttachmentRepository(JobFlowDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<AttachmentFile?> GetByUserIdAsync(Guid userId)
    {
        return await _context.AttachmentsFiles
            .FirstOrDefaultAsync(x => x.AppUserId == userId);
    }
}