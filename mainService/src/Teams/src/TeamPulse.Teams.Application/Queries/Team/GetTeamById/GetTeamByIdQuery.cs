using TeamPulse.Core.Abstractions;

namespace TeamPulse.Teams.Application.Queries.Team;

public record GetTeamByIdQuery(Guid TeamId) : IQuery;