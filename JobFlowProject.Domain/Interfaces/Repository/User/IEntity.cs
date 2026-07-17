using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities.User;

namespace JobFlowProject.Domain.Interfaces;

public interface IEntity
{
    public DateTime CreatedAt { get; }
    public Guid? CreatedById { get; }
    public AppUser? Creator { get; }
    public DateTime? ModifiedAt { get; }
    public Guid? ModifiedById { get; }
    public AppUser? Modifier { get; }
    public DateTime? DeletedAt { get; }
    public Guid? DeletedById { get; }
    public AppUser? Deleter { get; }
    public bool IsDeleted { get; }
    void SetAsDeleted(Guid requesterId);
    void SetModificationInfo(Guid requesterId);
    

    

   
}