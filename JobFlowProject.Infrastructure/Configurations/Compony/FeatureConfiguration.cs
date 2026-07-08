using JobFlowProject.Domain.Entites.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobFlowProject.Infrastructure;

public class FeatureConfiguration : BaseModelBuilderConfiguration<Feature>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<Feature> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);
        
        builder.Property(x => x.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
      

        builder.HasIndex(x => x.Name)
            .IsUnique();
        
        }
}