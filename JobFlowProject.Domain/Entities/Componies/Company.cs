using JobFlowProject.Domain.Entites;
using JobFlowProject.Domain.Entites.Resume;
using JobFlowProject.Domain.Entities.Componies;
using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities.Job;
using JobFlowProject.Domain.Entities.User;


namespace JobFlowProject.Domain.Entities.Componies;


public class Company : BaseEntity
{
    public Company()
    {
        
    }

    public Company(
        string name,
        string nationalId,
        Guid appUserId,
        Guid provinceId,
        Guid cityId,
        string address)
    {
        Name = name;
        NationalId = nationalId;
        AppUserId = appUserId;

        ProvinceId = provinceId;
        CityId = cityId;

        Address = address;
    }
    /// <summary>
    /// نام شرکت
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// شناسه ملی
    /// </summary>
    public string NationalId { get; set; }
    
    public Guid CityId { get; set; }
    public City City { get; set; }

    public Guid ProvinceId { get; set; }
    public Province Province { get; set; }

    /// <summary>
    /// آدرس
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// درباره شرکت
    /// </summary>
    public string? About { get; set; }
    
    /// <summary>
    /// تعداد آگهی رایگان مجاز
    /// </summary>
    public int FreeJobPostLimit { get; set; } 
    
   

    /// <summary>
    /// کاربر مرتبط با شرکت
    /// </summary>
    public Guid  AppUserId {get;set;}
    public AppUser AppUser { get; set; } 

    /// <summary>
    /// لیست آگهی‌های شرکت
    /// </summary>
    public ICollection<JobPost> JobPosts { get; set; } = new List<JobPost>();

   
    
    /// <summary>
    /// لیست نظراتی که برای این شرکت نوشته شده
    /// </summary>
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    
    public ICollection<AttachmentFile> Attachments { get; set; } = new List<AttachmentFile>();

    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new Exception("Name cannot be empty");

        if (Name.Length < 2)
            throw new Exception("Name must be at least 2 characters");

        if (Name.Length > 200)
            throw new Exception("Name cannot exceed 200 characters");

        if (string.IsNullOrWhiteSpace(NationalId))
            throw new Exception("NationalId cannot be empty");

        if (NationalId.Length != 10 && NationalId.Length != 11)
            throw new Exception("NationalId must be 10 or 11 digits");

        if (string.IsNullOrWhiteSpace(Address))
            throw new Exception("Address cannot be empty");
    }
}