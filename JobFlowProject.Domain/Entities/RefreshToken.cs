using JobFlowProject.Domain.Entities.User;

namespace JobFlowProject.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public RefreshToken() { }

    public RefreshToken(Guid userId, string token, DateTime expiresAt)
    {
        AppUserId = userId;
        Token = token;
        ExpiresAt = expiresAt;
        IsRevoked = false;
    }

    public Guid AppUserId { get; set; }
    public AppUser AppUser { get; set; }

    public string Token { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool IsRevoked { get; private set; }

    public void Revoke()
    {
        IsRevoked = true;
    }

    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Token))
            throw new Exception("Refresh token is required.");

        if (ExpiresAt <= DateTime.UtcNow)
            throw new Exception("Expiration date is invalid.");
    }
}