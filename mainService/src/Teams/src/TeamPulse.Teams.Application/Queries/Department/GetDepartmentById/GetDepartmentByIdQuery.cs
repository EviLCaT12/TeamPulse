using TeamPulse.Core.Abstractions;

namespace TeamPulse.Teams.Application.Queries.Department;

public record GetDepartmentByIdQuery(Guid DepartmentId) : IQuery;