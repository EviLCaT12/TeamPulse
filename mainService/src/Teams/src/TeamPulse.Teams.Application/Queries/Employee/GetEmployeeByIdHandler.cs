using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Application.DatabaseAbstraction;
using TeamPulse.Teams.Application.DatabaseAbstraction.Repositories.Read;
using TeamPulse.Teams.Contract.Dtos;

namespace TeamPulse.Teams.Application.Queries.Employee;

public class GetEmployeeByIdHandler : IQueryHandler<EmployeeDto, GetEmployeeByIdQuery>
{
    private readonly IEmployeeReadRepository _employeeReadRepository;
    private readonly ILogger<GetEmployeeByIdHandler> _logger;

    public GetEmployeeByIdHandler(IEmployeeReadRepository employeeReadRepository, ILogger<GetEmployeeByIdHandler> logger)
    {
        _employeeReadRepository = employeeReadRepository;
        _logger = logger;
    }
    public async Task<Result<EmployeeDto, ErrorList>> HandleAsync(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
    {
        var employee = await _employeeReadRepository.GetEmployees()
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