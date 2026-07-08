using JobFlowProject.Domain.Entites.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobFlowProject.Infrastructure;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
     

    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.CreatedAt);
        builder.HasQueryFilter(e => !e.IsDeleted);
        
        
        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(100);
      
        builder.Property(x => x.LastName)
            .IsRequired().
            HasMaxLength(100);
        
        builder.Property(x => x.About)
            .HasMaxLength(1000);
        

        builder.HasOne(x => x.Company)
            .WithMany(x => x.Profiles)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.FirstName);
        builder.HasIndex(x => x.LastName);
    }
        
    }
