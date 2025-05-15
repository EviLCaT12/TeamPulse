using TeamPulse.Core.Abstractions;

namespace TeamPulse.Teams.Application.Commands.Team.Delete;

public record DeleteCommand(Guid TeamId) : ICommand;