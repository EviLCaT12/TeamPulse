using TeamPulse.Core.Abstractions;

namespace TeamPulse.Teams.Application.Queries.Team;

public record GetByIdQuery(Guid TeamId) : IQuery;