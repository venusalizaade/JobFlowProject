
using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities.Componies;
using JobFlowProject.Domain.Enums;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobFlowProject.Infrastructure.Configurations.User;

public class ReviewConfiguration : BaseModelBuilderConfiguration<Review>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<Review> builder)
    {
        builder.Property(x => x.Comment)
            .IsRequired()
            .HasMaxLength(2000);
      
        builder.Property(x => x.Rating)
            .IsRequired();
        
        builder.Property(x => x.Status)
            .HasConversion<int>()
            .HasDefaultValue(ReviewStatusEnum.Pending);

        builder.Property(x => x.IsReported)
            .HasDefaultValue(false);

        builder.Property(x => x.ReportReason)
            .HasMaxLength(500);

        builder.HasIndex(x => x.Status);

        builder.HasOne(x => x.Company)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.JobSeeker)
            .WithMany()
            .HasForeignKey(x => x.JobSeekerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}