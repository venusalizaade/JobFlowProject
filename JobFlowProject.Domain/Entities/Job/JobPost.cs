using JobFlowProject.Domain.Entites; 
using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities;
using JobFlowProject.Domain.Entities.Componies;
using JobFlowProject.Domain.Entities.Componies.ComponyFeatures;
using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Domain.Entities.Job;

public class JobPost : BaseEntity
{
    public JobPost() { }

    public JobPost(string title, string aboutJob, Guid provinceId, Guid cityId,
        EmploymentTypeEnum employmentType,decimal? salary, Guid companyId, Guid categoryId ,
        Guid skillId)
    {
        Title = title;
        AboutJob = aboutJob;

        ProvinceId = provinceId;
        CityId = cityId;

        EmploymentType = employmentType;
        Salary = salary;
        CompanyId = companyId;
        CategoryId = categoryId;
        SkillId = skillId;

        IsActive = true;
        ExpiresAt = DateTime.UtcNow.AddDays(30);
    }

    

    /// <summary>
    /// عنوان شغل
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// توضیحات کامل شغل
    /// </summary>
    public string AboutJob { get; set; }
    public Guid CityId { get; set; }
    public City City { get; private set; }

    public Guid ProvinceId { get; set; }
    public Province Province { get; private set; }

    /// <summary>
    /// حقوق (اختیاری)
    /// </summary>
    public decimal? Salary { get; set; }

    /// <summary>
    /// نوع همکاری
    /// </summary>
    public EmploymentTypeEnum EmploymentType { get; set; }

    /// <summary>
    /// وضعیت فعال بودن آگهی
    /// </summary>
    public bool IsActive { get;  set; } = true;

    /// <summary>
    /// تاریخ انقضای آگهی
    /// </summary>
    public DateTime ExpiresAt { get;  private set; }

   
    public Guid CompanyId { get; set; }

    
    public Company Company { get;  private set; }


    public Guid CategoryId { get; set; }

    public Category Category { get;  private set; }
    
    public Guid SkillId { get; set; }

    public Skill Skill { get; set; }

    public ICollection<JobApplication> JobApplications { get;  private set; } = new List<JobApplication>();
    
    
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    
    public bool IsFeatured { get; private set; }

    public DateTime? FeaturedUntil { get; private set; }

    public void SetFeatured(int durationDays, Guid requesterId)
    {
        IsFeatured = true;
        FeaturedUntil = DateTime.UtcNow.AddDays(durationDays);
        SetModificationInfo(requesterId);
    }

    public void RemoveFeatured(Guid requesterId)
    {
        IsFeatured = false;
        FeaturedUntil = null;
        SetModificationInfo(requesterId);
    }
    
    public void Activate(Guid requesterId)
    {
        IsActive = true;
        SetModificationInfo(requesterId);
    }

    public void Deactivate(Guid requesterId)
    {
        IsActive = false;
        SetModificationInfo(requesterId);
    }

    public void ToggleStatus(Guid requesterId)
    {
        IsActive = !IsActive;
        SetModificationInfo(requesterId);
    }


    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Title))
            throw new Exception("Title cannot be empty");

        if (Title.Length < 3)
            throw new Exception("Title must be at least 3 characters");

        if (Title.Length > 200)
            throw new Exception("Title cannot exceed 200 characters");

        if (string.IsNullOrWhiteSpace(AboutJob))
            throw new Exception("AboutJob cannot be empty");

        if (AboutJob.Length < 10)
            throw new Exception("AboutJob must be at least 10 characters");

        if (ExpiresAt <= CreatedAt)
            throw new Exception("ExpiresAt must be after CreatedAt");
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}