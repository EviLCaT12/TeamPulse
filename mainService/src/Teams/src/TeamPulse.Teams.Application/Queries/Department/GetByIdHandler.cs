using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Application.DatabaseAbstraction;
using TeamPulse.Teams.Contract.Dtos;

namespace TeamPulse.Teams.Application.Queries.Department;

public class GetByIdHandler : IQueryHandler<DepartmentDto, GetByIdQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<GetByIdHandler> _logger;

    public GetByIdHandler(IReadDbContext readDbContext, ILogger<GetByIdHandler> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }
    
    public async Task<Result<DepartmentDto, ErrorList>> HandleAsync(GetByIdQuery query, CancellationToken cancellationToken)
    {
        var departments = await _readDbContext.Departments
            .Include(d => d.Teams)
            .Include(d => d.HeadOfDepartment)
            .FirstOrDefaultAsync(d => d.Id == query.DepartmentId, cancellationToken);

        if (departments is null)
        {
            var errorMessage = $"Department with id {query.DepartmentId} was not found";
            _logger.LogError(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }

        return departments;
    }
}