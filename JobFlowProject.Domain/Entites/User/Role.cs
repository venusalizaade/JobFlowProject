using JobFlowProject.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace JobFlowProject.Domain.Entites.User;

public sealed class Role : IdentityRole<Guid>, IEntity
{
    private Role()
    {

    }

    public Role(string roleName, Guid? requesterId = null) : base(roleName)
    {
        CreatedById = requesterId;
    }
    
    


    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public Guid? CreatedById { get; protected set; }
    public AppUser? Creator { get; protected set; }
    public DateTime? ModifiedAt { get; private set; }
    public Guid? ModifiedById { get; private set; }
    public AppUser? Modifier { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public Guid? DeletedById { get; set; }
    public AppUser? Deleter { get; set; }
    public bool IsDeleted { get; private set; }
    
    public void SetRoleName(string roleName, Guid requesterId)
    {
        Name = roleName;
        SetModificationInfo(requesterId);
    }
    
    public void SetAsDeleted(Guid requesterId)
    {
        DeletedAt = DateTime.UtcNow;
        IsDeleted = true;
        DeletedById = requesterId;
    }

    public void SetModificationInfo(Guid requesterId)
    {
        ModifiedAt = DateTime.UtcNow;
        ModifiedById = requesterId;
    }
}