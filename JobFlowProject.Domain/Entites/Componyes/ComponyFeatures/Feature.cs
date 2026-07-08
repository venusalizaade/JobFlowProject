using JobFlowProject.Domain.Entites.Job;
using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Domain.Entites.User;


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
    /// نوع فیچر
    /// </summary>
    public FeatureTypeEnum FeatureType { get; set; }

    public override void Validation()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new Exception("Name is required");

        if (Price < 0)
            throw new Exception("Price cannot be negative");
    }
}