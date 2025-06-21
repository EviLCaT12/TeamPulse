using TeamPulse.Core.Abstractions;

namespace TeamPulse.Accounts.Application.Commands.RefreshTokens;

public record RefreshTokensCommand(string AccessToken, Guid RefreshToken) : ICommand;