using JobFlowProject.Domain.Entites.Job;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobFlowProject.Infrastructure;

public class JobApplicationConfiguration : BaseModelBuilderConfiguration<JobApplication>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<JobApplication> builder)
    {
        builder.HasOne(x => x.JobPost)
            .WithMany(x => x.JobApplications)
            .HasForeignKey(x => x.JobPostId);

        builder.HasOne(x => x.JobSeeker)
            .WithMany(x => x.JobApplications)
            .HasForeignKey(x => x.JobSeekerId);


        builder.HasOne(x => x.Attachment)
            .WithMany()
            .HasForeignKey(x => x.AttachmentId)
            .OnDelete(DeleteBehavior.Restrict);
           

      
    }
}