using JobFlowProject.Domain.Entities.Componies.ComponyFeatures;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobFlowProject.Infrastructure.Configurations.Compony;

public class FeatureConfiguration : BaseModelBuilderConfiguration<Feature>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<Feature> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.DurationDays)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(x => x.Name)
            .IsUnique();
        
        }
}