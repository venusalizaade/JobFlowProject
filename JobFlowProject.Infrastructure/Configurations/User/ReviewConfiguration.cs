using JobFlowProject.Domain.Entites.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobFlowProject.Infrastructure;

public class ReviewConfiguration : BaseModelBuilderConfiguration<Review>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<Review> builder)
    {
        builder.Property(x => x.Comment)
            .IsRequired()
            .HasMaxLength(2000);
      
        builder.Property(x => x.Rating)
            .IsRequired();

        builder.HasOne(x => x.JobSeeker)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.JobSeekerId);

        builder.HasOne(x => x.Company)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.CompanyId);

     
    }
}