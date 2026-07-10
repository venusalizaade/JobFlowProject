using JobFlowProject.Domain.Interfaces;

namespace JobFlowProject.Domain.Entites;

public abstract class BaseEntity : IEntity
{
    public Guid Id { get;private set; }
    public DateTime? CreatedAt { get; private set; }=DateTime.UtcNow;
    public DateTime? DeletedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    
   
    public void SetAsDeleted()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }



    public abstract void Validation();



}