using TeamPulse.Core.Abstractions;

namespace TeamPulse.Teams.Application.Commands.Department.Create;

public record CreateCommand(
    string Name,
    IEnumerable<Guid>? Teams,
    Guid? HeadOfDepartment) : ICommand;