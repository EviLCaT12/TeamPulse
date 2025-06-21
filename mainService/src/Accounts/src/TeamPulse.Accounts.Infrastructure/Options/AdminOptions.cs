namespace TeamPulse.Accounts.Infrastructure.Options;

public class AdminOptions
{
    public const string SECTION_NAME = "ADMIN";

    public string UserName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}