using JobFlowProject.Domain.Entites.Resume;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobFlowProject.Infrastructure.Configurations.User;

public class AttachmentFileConfiguration : BaseModelBuilderConfiguration<AttachmentFile>
{
    protected override void ApplyEntityConfiguration(EntityTypeBuilder<AttachmentFile> builder)
    {
        builder.Property(x => x.FileName)
            .IsRequired()
            .HasMaxLength(255);
      
        builder.Property(x => x.FilePath)
            .IsRequired()
            .HasMaxLength(500);
      
        builder.Property(x => x.FileType)
            .IsRequired()
            .HasMaxLength(50);
      
        
        builder.HasOne(x => x.AppUser)
            .WithMany(x => x.Attachments)
            .HasForeignKey(x => x.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}