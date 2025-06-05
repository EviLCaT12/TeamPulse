using TeamPulse.Core.Abstractions;

namespace TeamPulse.Teams.Application.Queries.Employee;

public record GetEmployeeByIdQuery(Guid EmployeeId) : IQuery;
