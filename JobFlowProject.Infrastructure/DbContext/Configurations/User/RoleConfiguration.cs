using JobFlowProject.Domain.Entites.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobFlowProject.Infrastructure.DbContext.Configurations.User;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
       
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.CreatedAt);
        builder.HasQueryFilter(e => !e.IsDeleted);

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
        
    }
}