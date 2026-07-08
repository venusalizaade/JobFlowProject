namespace JobFlowProject.Domain.Entites.Job;


public class Category : BaseEntity
{
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

    public override void Validation()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new Exception("Category name is required");
    }
}

