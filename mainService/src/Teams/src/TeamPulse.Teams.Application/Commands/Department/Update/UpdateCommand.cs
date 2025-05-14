using TeamPulse.Core.Abstractions;

namespace TeamPulse.Teams.Application.Commands.Department.Update;

public record UpdateCommand(
    Guid DepartmentId,
    string? NewName,
    IEnumerable<Guid>? NewTeams,
    Guid? NewHeadOfDepartment) : ICommand;