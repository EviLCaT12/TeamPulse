using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Application.DatabaseAbstraction;

namespace TeamPulse.Teams.Application.Queries.Department.GetAllTeamsFromDepartment;

public class GetAllTeamsFromDepartmentHandler : IQueryHandler<List<Guid>, GetAllTeamsFromDepartmentQuery>
{
    private readonly ILogger<GetAllTeamsFromDepartmentHandler> _logger;
    private readonly IReadDbContext _readDbContext;

    public GetAllTeamsFromDepartmentHandler(ILogger<GetAllTeamsFromDepartmentHandler> logger,
        IReadDbContext readDbContext)
    {
        _logger = logger;
        _readDbContext = readDbContext;
    }

    public async Task<Result<List<Guid>, ErrorList>> HandleAsync(GetAllTeamsFromDepartmentQuery query,
        CancellationToken cancellationToken)
    {
        var teamsId = await _readDbContext.Teams
            .Where(t => t.DepartmentId == query.DepartmentId)
            .Select(t => t.Id)
            .ToListAsync(cancellationToken);

        if (teamsId.Count == 0)
        {
            var errorMessage = $"No teams found for department {query.DepartmentId}";
            _logger.LogError(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }

        return teamsId;
    }
}