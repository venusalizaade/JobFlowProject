
using JobFlowProject.Domain.Entities.Job;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
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
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        
       

        builder.HasOne(x => x.Company)
            .WithMany(x => x.JobPosts)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasOne(x => x.Category)
            .WithMany(x => x.JobPosts)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.Province)
            .WithMany()
            .HasForeignKey(x => x.ProvinceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.City)
            .WithMany()
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.Skill)
            .WithMany(x => x.JobPosts)
            .HasForeignKey(x => x.SkillId)
            .OnDelete(DeleteBehavior.Restrict);
        
    }
}