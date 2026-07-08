using JobFlowProject.Domain.Entites.Job;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobFlowProject.Infrastructure;

public class JobPostConfiguration : BaseModelBuilderConfiguration<JobPost>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<JobPost> builder)
    {
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);
     
        builder.Property(x => x.AboutJob)
            .IsRequired()
            .HasMaxLength(4000);
        
        builder.Property(x => x.Salary)
            .HasMaxLength(100);

        builder.HasOne(x => x.Company)
            .WithMany(x => x.JobPosts)
            .HasForeignKey(x => x.CompanyId);


        builder.HasOne(x => x.Employer)
            .WithMany(x => x.JobPosts)
            .HasForeignKey(x => x.EmployerId);
           

        builder.HasOne(x => x.Category)
            .WithMany(x => x.JobPosts)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        
    }
}