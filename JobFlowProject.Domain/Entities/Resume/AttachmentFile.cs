
using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities;
using JobFlowProject.Domain.Entities.Componies;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Domain.Entites.Resume;

public class AttachmentFile : BaseEntity
{
    private AttachmentFile() { }
   

    public AttachmentFile(string fileName, string filePath, string fileType, Guid profileId,
        AttachmentType attachmentType, byte[]? fileData = null)
    {
        FileName = fileName;
        FilePath = filePath;
        FileType = fileType;
        FileData = fileData;
        AppUserId = profileId;
        AttachmentType = attachmentType;
        Validate();
    }

    public AttachmentFile(string fileName, string filePath, string fileType, Guid requesterId)
    {
        FileName = fileName;
        FilePath = filePath;
        FileType = fileType;
        AppUserId = requesterId;
    }

    /// <summary>
    /// نام فایل
    /// </summary>
    public string FileName { get;  set; }

    /// <summary>
    /// مسیر ذخیره‌سازی فایل
    /// </summary>
    public string FilePath { get; private set; }

    /// <summary>
    /// داده‌ی فایل (در صورت ذخیره در دیتابیس)
    /// </summary>
    public byte[]? FileData { get; private set; }

    /// <summary>
    /// نوع فایل (pdf, jpg, ...)
    /// </summary>
    public string FileType { get; private set; }
    
    /// <summary>
    ///آیدی کاربر
    /// </summary>
    public Guid AppUserId { get; private set; }

    public AttachmentType AttachmentType { get; private set; }
    /// <summary>
    /// کاربر
    /// </summary>
    public AppUser AppUser { get; private set; }
    
    public Guid? CompanyId { get; private set; }
    public Company? Company { get; private set; }

    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(FileName))
            throw new Exception("FileName is required");

        if (string.IsNullOrWhiteSpace(FilePath) && FileData == null)
            throw new Exception("Either FilePath or FileData is required");

       
    }
    public void SetFile(string fileName, string filePath, string fileType)
    {
        FileName = fileName;
        FilePath = filePath;
        FileType = fileType;
    }
    
}