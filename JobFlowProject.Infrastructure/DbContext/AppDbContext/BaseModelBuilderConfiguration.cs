using JobFlowProject.Domain.Entites;
using JobFlowProject.Domain.Entities;
using JobFlowProject.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobFlowProject.Infrastructure.DbContext.AppDbContext;

public abstract class BaseModelBuilderConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity 
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
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
        
        ApplyEntityConfiguration(builder);
    }

    protected abstract void ApplyEntityConfiguration(EntityTypeBuilder<TEntity> modelBuilder);
}
