using JobFlowProject.Domain.Entities;
using JobFlowProject.Domain.Entities.Job;


namespace JobFlowProject.Domain.Entities.Job;


public class Category : BaseEntity
{
    private Category()
    {
        
        
    }
    public Category(string name, string description)
    {
        Name = name;
        Description = description;
        
    }
    /// <summary>
    /// نام دسته‌بندی
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// توضیحات
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// لیست آگهی‌های این دسته‌بندی
    /// </summary>
    public ICollection<JobPost> JobPosts { get; set; } = new List<JobPost>();
  
    public ICollection<Skill> Skills { get; private set; } = new List<Skill>();

    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new Exception("Category name is required");
    }
}

