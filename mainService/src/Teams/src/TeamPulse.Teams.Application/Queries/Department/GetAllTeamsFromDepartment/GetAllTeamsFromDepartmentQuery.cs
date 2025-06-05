using TeamPulse.Core.Abstractions;

namespace TeamPulse.Teams.Application.Queries.Department.GetAllTeamsFromDepartment;

public record GetAllTeamsFromDepartmentQuery(Guid DepartmentId) : IQuery;