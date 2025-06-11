using TeamPulse.Core.Abstractions;

namespace TeamPulse.Teams.Application.Queries.Team.GetTeamById;

public record GetTeamByIdQuery(Guid TeamId) : IQuery;