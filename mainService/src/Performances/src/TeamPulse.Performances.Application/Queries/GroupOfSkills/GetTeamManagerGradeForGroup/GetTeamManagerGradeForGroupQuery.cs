using TeamPulse.Core.Abstractions;

namespace TeamPulse.Performances.Application.Queries.GroupOfSkills.GetTeamManagerGradeForGroup;

public record GetTeamManagerGradeForGroupQuery(List<Guid> EmployeesIds, Guid GroupId) : IQuery;