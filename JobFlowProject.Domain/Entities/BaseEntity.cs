using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Interfaces;

namespace JobFlowProject.Domain.Entities;

public abstract class BaseEntity : IEntity
{
    public Guid Id { get; set; }
    public abstract void Validate();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public Guid? CreatedById { get; protected set; }
    public AppUser? Creator { get; protected set; }
    public DateTime? ModifiedAt { get; private set; }
    public Guid? ModifiedById { get; private set; }
    public AppUser? Modifier { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public Guid? DeletedById { get;private  set; }
    public AppUser? Deleter { get;private  set; }
    public bool IsDeleted { get; private set; }
  
    public void SetAsDeleted(Guid requesterId)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedById = requesterId;
    }

    public void SetModificationInfo(Guid requesterId)
    {
        ModifiedAt = DateTime.UtcNow;
        ModifiedById = requesterId;
    }
}

