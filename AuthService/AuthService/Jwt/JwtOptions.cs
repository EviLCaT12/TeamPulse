namespace AuthService.Jwt;

public class JwtOptions
{
    public const string SECTION_NAME = "JwtOptions";
    
    public string SecretKey { get; init; } = string.Empty;
    
    public int TokenLifetimeInMinute { get; init; } 
    
    public string Issuer { get; init; } = string.Empty;
}