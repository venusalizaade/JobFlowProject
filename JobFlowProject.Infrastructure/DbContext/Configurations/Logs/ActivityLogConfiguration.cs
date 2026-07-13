using JobFlowProject.Domain.Entites.Logs;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobFlowProject.Infrastructure;

public class ActivityLogConfiguration : BaseModelBuilderConfiguration<ActivityLog>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<ActivityLog> builder)
    {
        builder.Property(x => x.Action)
            .IsRequired()
            .HasMaxLength(200);
       
        builder.Property(x => x.Details)
            .HasMaxLength(1000);
       
        builder.Property(x => x.EntityType)
            .HasMaxLength(100);

        builder.HasOne(x => x.Profile)
            .WithMany()
            .HasForeignKey(x => x.ProfileId)
            .OnDelete(DeleteBehavior.Restrict);

      
    }
}