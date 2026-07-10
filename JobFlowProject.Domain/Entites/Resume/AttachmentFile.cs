using JobFlowProject.Domain.Entites.Job;
using JobFlowProject.Domain.Entites.User;

namespace JobFlowProject.Domain.Entites.Resume;

public class AttachmentFile : BaseEntity
{
    /// <summary>
    /// نام فایل
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// مسیر ذخیره‌سازی فایل
    /// </summary>
    public string FilePath { get; set; }

    /// <summary>
    /// داده‌ی فایل (در صورت ذخیره در دیتابیس)
    /// </summary>
    public byte[]? FileData { get; set; }

    /// <summary>
    /// نوع فایل (pdf, jpg, ...)
    /// </summary>
    public string FileType { get; set; }
    
    /// <summary>
    ///آیدی کاربر
    /// </summary>
    public Guid ProfileId { get; set; }

    /// <summary>
    /// کاربر
    /// </summary>
    public AppUser AppUser { get; set; }

    public override void Validation()
    {
        if (string.IsNullOrWhiteSpace(FileName))
            throw new Exception("FileName is required");

        if (string.IsNullOrWhiteSpace(FilePath) && FileData == null)
            throw new Exception("Either FilePath or FileData is required");

        if (ProfileId == Guid.Empty)
            throw new Exception("Profile is required");
    }

    
}