using TeamPulse.Core.Abstractions;

namespace TeamPulse.Teams.Application.Commands.Department.Create;

public record CreateDepartmentCommand(
    string Name,
    IEnumerable<Guid>? Teams,
    Guid HeadOfDepartment) : ICommand;