using JobFlowProject.Domain.Interfaces;

namespace JobFlowProject.Domain.Entites;

public abstract class BaseEntity : IEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }=DateTime.UtcNow;
    public DateTime DeletedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    
   
    public void SetAsDeleted()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }



    public abstract void Validation();



}