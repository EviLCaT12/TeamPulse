using TeamPulse.Core.Abstractions;

namespace TeamPulse.Teams.Application.Queries.Team.GetAllEmployeesFromTeam;

public record GetAllEmployeesFromTeamQuery(Guid TeamId) : IQuery;