using JobFlowProject.Domain.Entites;
using JobFlowProject.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobFlowProject.Infrastructure;

public abstract class BaseModelBuilderConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity , IEntity
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();
             
        builder.HasIndex(x => x.CreatedAt);
        builder.HasQueryFilter(e => !e.IsDeleted);
        
        ApplyEntityConfiguration(builder);
    }

    protected abstract void ApplyEntityConfiguration(EntityTypeBuilder<TEntity> modelBuilder);
}
