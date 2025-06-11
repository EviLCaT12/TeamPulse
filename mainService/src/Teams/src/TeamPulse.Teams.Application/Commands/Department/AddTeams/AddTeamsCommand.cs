using TeamPulse.Core.Abstractions;

namespace TeamPulse.Teams.Application.Commands.Department.AddTeams;

public record AddTeamsCommand(
    Guid DepartmentId,
    List<Guid> TeamIds) : ICommand;