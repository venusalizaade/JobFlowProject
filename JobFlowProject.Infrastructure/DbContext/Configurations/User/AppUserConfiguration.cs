using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobFlowProject.Infrastructure.DbContext.Configurations.User;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        
         builder.HasIndex(x => x.CreatedAt);
        builder.HasQueryFilter(e => !e.IsDeleted);

        builder.Property(x => x.NationalId)
            .IsRequired()
            .HasMaxLength(10);
        
        builder.HasIndex(x => x.NationalId)
            .IsUnique();
       

        builder.Property(x => x.FirstName)
            .HasMaxLength(100);
        builder.Property(x => x.LastName)
            .HasMaxLength(100);
        builder.Property(x => x.IsApproved)
            .HasDefaultValue(false);

        builder.HasOne(x => x.Creator)
            .WithMany()
            .HasForeignKey(u => u.CreatedById)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(x => x.Deleter)
            .WithMany()
            .HasForeignKey(u => u.DeletedById)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(x => x.Modifier)
            .WithMany()
            .HasForeignKey(u => u.ModifiedById)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(x => x.Company)
            .WithOne(x => x.AppUser)
            .HasForeignKey<AppUser>(x => x.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);
       
    }
}