namespace JobFlowProject.Domain.Entities;

public class City : BaseEntity
{
    

    public City(string name, Guid provinceId)
    {
        Name = name;
        ProvinceId = provinceId;
    }

    public City()
    {
        
    }

    /// <summary>
    /// نام شهر
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// استان
    /// </summary>
    public Guid ProvinceId { get; private set; }

    public Province Province { get; private set; }

    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new Exception("City name cannot be empty.");
    }
}