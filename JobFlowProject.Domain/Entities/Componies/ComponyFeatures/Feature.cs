using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Domain.Entities.Componies.ComponyFeatures;


public class Feature : BaseEntity
{
    public Feature(
        string name,
        decimal price,
        int durationDays,
        FeatureTypeEnum featureType,
        string? description = null,
        bool isActive = true)
    {
        Name = name;
        Price = price;
        DurationDays = durationDays;
        FeatureType = featureType;
        Description = description;
        IsActive = isActive;

        Validate();
    }

    private Feature() { }
    
    /// <summary>
    /// نام 
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// توضیحات فیچر
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// قیمت به تومان
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// تعداد روزهای قابل استفاده فیچر
    /// </summary>
    public int DurationDays { get; set; }

    /// <summary>
    /// نوع فیچر
    /// </summary>
    public FeatureTypeEnum FeatureType { get; set; }

    /// <summary>
    /// آیا فیچر قابل نمایش / فروش است؟
    /// </summary>
    public bool IsActive { get; set; }

    public ICollection<CompanyFeature> CompanyFeatures { get; set; } = new List<CompanyFeature>();
    public ICollection<JobFeature> JobFeatures { get; set; } = new List<JobFeature>();

    public void ToggleActive()
    {
        IsActive = !IsActive;
    }

    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new Exception("Name cannot be empty or whitespace");

        if (Name.Length < 3)
            throw new Exception("Name must be at least 3 characters");

        if (Name.Length > 150)
            throw new Exception("Name cannot exceed 150 characters");

        if (Description is { Length: > 1000 })
            throw new Exception("Description cannot exceed 1000 characters");

        if (Price <= 0)
            throw new Exception("Price must be greater than zero");

        if (DurationDays < 0)
            throw new Exception("DurationDays cannot be negative");

        if (DurationDays > 365)
            throw new Exception("DurationDays cannot exceed 365 days");

        if (!Enum.IsDefined(typeof(FeatureTypeEnum), FeatureType))
            throw new Exception("FeatureType is invalid");
    
    }
}
