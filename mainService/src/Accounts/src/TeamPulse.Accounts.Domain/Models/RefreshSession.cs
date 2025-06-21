namespace TeamPulse.Accounts.Domain.Models;

public class RefreshSession
{
    
    public Guid Id { get; init; }
    
    public Guid UserId { get; init; }
    
    public User User { get; init; }
    
    public Guid RefreshTokenId { get; init; }
    
    public Guid Jti {get; init; }
    
    public DateTime ExpiresIn { get; init; }
    
    public DateTime CreatedAt { get; init; }
}