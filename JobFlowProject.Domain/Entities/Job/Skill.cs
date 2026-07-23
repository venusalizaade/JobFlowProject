namespace JobFlowProject.Domain.Entities.Job;

public class Skill : BaseEntity
{
    private Skill() { }

    public Skill(string name, Guid categoryId)
    {
        Name = name;
        CategoryId = categoryId;

        Validate();
    }

    public string Name { get; private set; }

    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; }

    public ICollection<JobPost> JobPosts { get; private set; } = new List<JobPost>();

    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new Exception("Skill name is required.");
    }
}