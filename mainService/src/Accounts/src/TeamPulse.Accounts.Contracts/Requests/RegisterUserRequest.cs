namespace TeamPulse.Accounts.Contracts.Requests;

public record RegisterUserRequest(
    string Email,
    string Name,
    string Password);