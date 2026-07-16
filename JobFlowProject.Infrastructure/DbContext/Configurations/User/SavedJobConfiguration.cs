using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobFlowProject.Infrastructure.Configurations.User;

public class SavedJobConfiguration : BaseModelBuilderConfiguration<SavedJob>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<SavedJob> builder)
    {
        builder.HasOne(x => x.AppUser)
            .WithMany(x => x.SavedJobs)
            .HasForeignKey(x => x.AppUserId);
        
            
        builder.HasIndex(x => new { x.AppUserId, x.JobPostId })
            .IsUnique();
    }
}