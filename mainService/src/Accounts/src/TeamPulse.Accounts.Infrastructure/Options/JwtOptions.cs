namespace TeamPulse.Accounts.Infrastructure.Options;

public class JwtOptions
{
    public const string SECTION_NAME = "JwtOptions";
    
    public string Issuer { get; init; } = string.Empty;
    
    public string Audience { get; init; } = string.Empty;
    
    public string Key { get; init; } = string.Empty;
    
    public int ExpiredMinutesTime { get; init; } 
}