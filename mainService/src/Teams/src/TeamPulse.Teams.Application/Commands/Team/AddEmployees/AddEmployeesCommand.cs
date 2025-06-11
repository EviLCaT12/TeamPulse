using TeamPulse.Core.Abstractions;

namespace TeamPulse.Teams.Application.Commands.Team.AddEmployees;

public record AddEmployeesCommand(Guid TeamId, IEnumerable<Guid> EmployeeIds) : ICommand;