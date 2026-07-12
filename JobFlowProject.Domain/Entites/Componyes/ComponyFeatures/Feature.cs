using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Domain.Entites.Componyes.ComponyFeatures;


public class Feature : BaseEntity
{
    /// <summary>
    /// نام 
    /// </summary>
    public string Name { get; set; }
    

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

    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new Exception("Name cannot be empty or whitespace");

        if (Name.Length < 3)
            throw new Exception("Name must be at least 3 characters");

        if (Name.Length > 150)
            throw new Exception("Name cannot exceed 150 characters");

        if (Price <= 0)
            throw new Exception("Price must be greater than zero");

        if (DurationDays < 0)
            throw new Exception("DurationDays cannot be negative");

        if (DurationDays > 62)
            throw new Exception("DurationDays cannot exceed 62 days");
    
    }
}