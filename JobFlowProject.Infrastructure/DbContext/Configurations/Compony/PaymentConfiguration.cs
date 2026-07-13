using JobFlowProject.Domain.Entites.Componyes.ComponyFeatures;
using JobFlowProject.Domain.Entites.Job;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobFlowProject.Infrastructure;

public class PaymentConfiguration : BaseModelBuilderConfiguration<Payment>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<Payment> builder)
    {
        builder.Property(x => x.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        
        
        builder.HasOne(x => x.JobPost)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.JobPostId)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        builder.HasOne(x => x.Feature)
            .WithMany()
            .HasForeignKey(x => x.FeatureId)
            .OnDelete(DeleteBehavior.Restrict) ;


    }
}