using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Application.DatabaseAbstraction;
using TeamPulse.Teams.Contract.Dtos;

namespace TeamPulse.Teams.Application.Queries.Employee;

public class GetByIdHandler : IQueryHandler<EmployeeDto, GetByIdQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<GetByIdHandler> _logger;

    public GetByIdHandler(IReadDbContext readDbContext, ILogger<GetByIdHandler> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }
    public async Task<Result<EmployeeDto, ErrorList>> HandleAsync(GetByIdQuery query, CancellationToken cancellationToken)
    {
        var employee = await _readDbContext.Employees
            .FirstOrDefaultAsync(e => e.Id == query.EmployeeId, cancellationToken);

        if (employee is null)
        {
            var errorMessage = $"Employee with Id {query.EmployeeId} does not exist.";
            _logger.LogError(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }

        return employee;
    }
}