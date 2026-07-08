using JobFlowProject.Domain.Entites.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobFlowProject.Infrastructure;

public class SavedJobConfiguration : BaseModelBuilderConfiguration<SavedJob>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<SavedJob> builder)
    {
        builder.HasOne(x => x.Profile)
            .WithMany(x => x.SavedJobs)
            .HasForeignKey(x => x.ProfileId);

        builder.HasOne(x => x.JobPost)
            .WithMany(x => x.SavedJobs)
            .HasForeignKey(x => x.JobPostId);
            
        builder.HasIndex(x => new { x.ProfileId, x.JobPostId })
            .IsUnique();
    }
}