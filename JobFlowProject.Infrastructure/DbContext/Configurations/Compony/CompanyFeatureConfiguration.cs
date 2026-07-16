using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities.Componies.ComponyFeatures;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobFlowProject.Infrastructure;

public class CompanyFeatureConfiguration : BaseModelBuilderConfiguration<CompanyFeature>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<CompanyFeature> builder)
    {
        builder.HasOne(x => x.Company)
            .WithMany()
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Feature)
            .WithMany()
            .HasForeignKey(x => x.FeatureId)
            .OnDelete(DeleteBehavior.Restrict);



    }
}