using TeamPulse.Core.Abstractions;

namespace TeamPulse.Teams.Application.Commands.Department.AddTeamsToDepartment;

public record AddTeamsToDepartmentCommand(
    Guid DepartmentId,
    List<Guid> TeamIds) : ICommand;