using TeamPulse.Core.Abstractions;

namespace TeamPulse.Accounts.Application.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand;