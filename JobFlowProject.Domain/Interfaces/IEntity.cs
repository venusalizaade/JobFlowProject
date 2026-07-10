namespace JobFlowProject.Domain.Interfaces;

public interface IEntity
{
    public DateTime? CreatedAt { get; } 
    public DateTime? DeletedAt { get; }
    public DateTime UpdatedAt { get;  }
    public bool IsDeleted { get;  }
    

    

   
}