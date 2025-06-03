using TeamPulse.Core.Abstractions;

namespace TeamPulse.Performances.Application.Queries.GroupOfSkills.GetDepartmentManagerGradeForGroup;

public record GetDepartmentManagerGradeForGroupQuery(IEnumerable<Guid> TeamIds, Guid GroupId) : IQuery;