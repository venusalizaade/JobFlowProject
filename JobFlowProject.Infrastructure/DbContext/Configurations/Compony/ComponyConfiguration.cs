using JobFlowProject.Domain.Entites.Componyes;
using JobFlowProject.Domain.Entites.Job;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobFlowProject.Infrastructure;

public class CompanyConfiguration : BaseModelBuilderConfiguration<Company>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<Company> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);
      
        builder.Property(x => x.NationalId)
            .IsRequired()
            .HasMaxLength(15);
      
        builder.Property(x => x.Address)
            .IsRequired().HasMaxLength(500);
      
        builder.Property(x => x.About)
            .HasMaxLength(2000);

        builder.HasIndex(x => x.NationalId)
            .IsUnique();
        
       
    }
}