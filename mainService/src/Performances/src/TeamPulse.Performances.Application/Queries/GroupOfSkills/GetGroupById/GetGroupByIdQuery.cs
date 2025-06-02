using TeamPulse.Core.Abstractions;

namespace TeamPulse.Performances.Application.Queries.GroupOfSkills.GetGroupById;

public record GetGroupByIdQuery(Guid GroupId) : IQuery;