using JobFlowProject.Domain.Entites.Resume;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobFlowProject.Infrastructure;

public class ResumeBuilderConfiguration : BaseModelBuilderConfiguration<ResumeBuilder>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<ResumeBuilder> builder)
    {
 
        builder.Property(x => x.Address)
            .HasMaxLength(500);
     
        builder.Property(x => x.City)
            .HasMaxLength(100);
       
        builder.Property(x => x.Education)
            .HasMaxLength(1000);
      
        builder.Property(x => x.Experience)
            .HasMaxLength(2000);
        
        builder.Property(x => x.About)
            .HasMaxLength(2000);

        builder.HasOne(x => x.Profile)
            .WithMany()
            .HasForeignKey(x => x.ProfileId);



    }
}