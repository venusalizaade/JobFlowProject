using JobFlowProject.Domain.Entites.Componyes.ComponyFeatures;
using JobFlowProject.Domain.Entites.Job;
using JobFlowProject.Domain.Entites.User;

namespace JobFlowProject.Domain.Entites.Componyes;


public class Company : BaseEntity
{ 
    
    /// <summary>
    /// نام شرکت
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// شناسه ملی
    /// </summary>
    public string NationalId { get; set; }
    
    public string City { get; set; }
    
    public string Province { get; set; }

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
    
    public AppUser AppUser { get; set; } 

    /// <summary>
    /// لیست آگهی‌های شرکت
    /// </summary>
    public ICollection<JobPost> JobPosts { get; set; } = new List<JobPost>();

   
    
    /// <summary>
    /// لیست نظراتی که برای این شرکت نوشته شده
    /// </summary>
    public ICollection<Review> Reviews { get; set; } = new List<Review>();

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