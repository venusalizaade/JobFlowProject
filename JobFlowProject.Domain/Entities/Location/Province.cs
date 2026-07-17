namespace JobFlowProject.Domain.Entities;

public class Province : BaseEntity
{
    private Province() { }

    public Province(string name)
    {
        Name = name;
    }

    /// <summary>
    /// نام استان
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// شهرهای استان
    /// </summary>
    public ICollection<City> Cities { get; private set; } = new List<City>();

    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new Exception("Province name cannot be empty.");
    }
}