using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Application.DatabaseAbstraction;
using TeamPulse.Teams.Application.DatabaseAbstraction.Repositories.Read;

namespace TeamPulse.Teams.Application.Queries.Team.GetAllEmployeesFromTeam;

public class GetAllEmployeesFromTeamHandler : IQueryHandler<List<Guid>, GetAllEmployeesFromTeamQuery>
{
    private readonly ILogger<GetAllEmployeesFromTeamHandler> _logger;
    private readonly IEmployeeReadRepository _employeeReadRepository;


    public GetAllEmployeesFromTeamHandler(ILogger<GetAllEmployeesFromTeamHandler> logger,
        IEmployeeReadRepository employeeReadRepository)
    {
        _logger = logger;
        _employeeReadRepository = employeeReadRepository;
    }

    public async Task<Result<List<Guid>, ErrorList>> HandleAsync(GetAllEmployeesFromTeamQuery query,
        CancellationToken cancellationToken)
    {
        var employees = await _employeeReadRepository.GetEmployees()
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