using TeamPulse.Core.Abstractions;

namespace TeamPulse.Teams.Application.Queries.Employee;

public record GetByIdQuery(Guid EmployeeId) : IQuery;
