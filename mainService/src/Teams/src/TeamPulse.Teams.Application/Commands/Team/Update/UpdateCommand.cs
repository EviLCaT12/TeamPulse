using TeamPulse.Core.Abstractions;

namespace TeamPulse.Teams.Application.Commands.Team.Update;

public record UpdateCommand(
    Guid TeamId,
    string? NewName,
    Guid? NewDepartmentId,
    IEnumerable<Guid>? NewEmployees,
    Guid? NewHeadOfTeam) : ICommand;