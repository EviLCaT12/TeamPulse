using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Application.DatabaseAbstraction;

namespace TeamPulse.Teams.Application.Queries.Team.GetAllEmployeesFromTeam;

public class GetAllEmployeesFromTeamHandler : IQueryHandler<List<Guid>, GetAllEmployeesFromTeamQuery>
{
    private readonly ILogger<GetAllEmployeesFromTeamHandler> _logger;
    private readonly IReadDbContext _readDbContext;

    public GetAllEmployeesFromTeamHandler(ILogger<GetAllEmployeesFromTeamHandler> logger, IReadDbContext readDbContext)
    {
        _logger = logger;
        _readDbContext = readDbContext;
    }

    public async Task<Result<List<Guid>, ErrorList>> HandleAsync(GetAllEmployeesFromTeamQuery query,
        CancellationToken cancellationToken)
    {
        var employees = await _readDbContext.Employees
            .Where(e => e.TeamId == query.TeamId)
            .Select(e => e.Id)
            .ToListAsync(cancellationToken);

        if (employees.Any() == false)
        {
            var errorMessage = $"No employees found for team {query.TeamId}";
            _logger.LogError(errorMessage);
            Errors.General.ValueNotFound(errorMessage);
        }

        return employees;
    }
}