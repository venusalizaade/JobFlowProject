using JobFlowProject.Domain.Entites.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobFlowProject.Infrastructure.Configurations.User;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
     

    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.CreatedAt);
        builder.HasQueryFilter(e => !e.IsDeleted);

        builder.HasOne(u => u.Company)
            .WithOne(c => c.AppUser)
            .HasForeignKey<AppUser>(u => u.CompanyId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne(x => x.Deleter)
            .WithMany()
            .HasForeignKey(u => u.DeletedById)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(x => x.Modifier)
            .WithMany()
            .HasForeignKey(u => u.ModifiedById)
            .OnDelete(DeleteBehavior.NoAction);
        
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
