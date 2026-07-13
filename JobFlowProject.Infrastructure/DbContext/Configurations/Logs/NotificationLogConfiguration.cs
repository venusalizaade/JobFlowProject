using JobFlowProject.Domain.Entites.Logs;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobFlowProject.Infrastructure;

public class NotificationLogConfiguration : BaseModelBuilderConfiguration<NotificationLog>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<NotificationLog> builder)
    {
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);
      
        builder.Property(x => x.Message)
            .IsRequired()
            .HasMaxLength(2000);
      
        builder.Property(x => x.SentAt)
            .IsRequired();

        builder.HasOne(x => x.AppUser)
            .WithMany(x => x.Notifications)
            .HasForeignKey(x => x.ProfileId);

        builder.HasOne(x => x.Company)
            .WithMany()
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

       
    }
}