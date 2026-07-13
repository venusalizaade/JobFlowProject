using JobFlowProject.Domain.Entites.Job;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobFlowProject.Infrastructure;

public class CategoryConfiguration : BaseModelBuilderConfiguration<Category>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<Category> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);
        
        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}