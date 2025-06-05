using TeamPulse.Core.Abstractions;

namespace TeamPulse.Performances.Application.Queries.GroupOfSkills.GetEmployeeManagerGradeForGroup;

public record GetEmployeeManagerGradeForGroupQuery(Guid EmployeeId, Guid GroupId) : IQuery;