using JobFlowProject.Domain.Entites.Componyes;
using JobFlowProject.Domain.Entites.User;
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
        ;

        builder.HasOne(x => x.Company)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

     
    }
}