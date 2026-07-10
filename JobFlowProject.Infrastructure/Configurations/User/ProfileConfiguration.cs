using JobFlowProject.Domain.Entites.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobFlowProject.Infrastructure.Configurations.User;

public class ProfileConfiguration : IEntityTypeConfiguration<AppUser>
{
     

    public void Configure(EntityTypeBuilder<AppUser> builder)
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
        
        
    }
        
    }
