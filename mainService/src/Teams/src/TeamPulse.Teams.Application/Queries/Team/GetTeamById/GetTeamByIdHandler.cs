using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Application.DatabaseAbstraction;
using TeamPulse.Teams.Contract.Dtos;

namespace TeamPulse.Teams.Application.Queries.Team;

public class GetTeamByIdHandler : IQueryHandler<TeamDto, GetTeamByIdQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<GetTeamByIdHandler> _logger;

    public GetTeamByIdHandler(IReadDbContext readDbContext, ILogger<GetTeamByIdHandler> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }
    public async Task<Result<TeamDto, ErrorList>> HandleAsync(GetTeamByIdQuery query, CancellationToken cancellationToken)
    {
        var team = await _readDbContext.Teams
            .Include(t => t.Employees)
            .FirstOrDefaultAsync(t => t.Id == query.TeamId, cancellationToken);

        if (team is null)
        {
            var errorMessage = $"Team with id {query.TeamId} does not exist.";
            _logger.LogError(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }

        return team;
    }
}