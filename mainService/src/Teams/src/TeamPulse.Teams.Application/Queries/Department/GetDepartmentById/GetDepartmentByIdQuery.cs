using TeamPulse.Core.Abstractions;

namespace TeamPulse.Teams.Application.Queries.Department.GetDepartmentById;

public record GetDepartmentByIdQuery(Guid DepartmentId) : IQuery;