using TeamPulse.Core.Abstractions;

namespace TeamPulse.Teams.Application.Queries.Department;

public record GetByIdQuery(Guid DepartmentId) : IQuery;