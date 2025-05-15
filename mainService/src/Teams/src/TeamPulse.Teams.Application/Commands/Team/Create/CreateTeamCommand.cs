using TeamPulse.Core.Abstractions;

namespace TeamPulse.Teams.Application.Commands.Team.Create;

public record CreateTeamCommand(
    string Name,
    Guid DepartmentId,
    IEnumerable<Guid>? EmployeeIds,
    Guid? HeadOfTeamId) : ICommand;