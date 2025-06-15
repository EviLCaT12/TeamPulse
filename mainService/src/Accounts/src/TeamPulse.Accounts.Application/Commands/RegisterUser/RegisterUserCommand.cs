using TeamPulse.Core.Abstractions;

namespace TeamPulse.Accounts.Application.Commands.RegisterUser;

public record RegisterUserCommand(
    string Email,
    string Name, //ToDO: Заменить на FioDto
    string Password) : ICommand;