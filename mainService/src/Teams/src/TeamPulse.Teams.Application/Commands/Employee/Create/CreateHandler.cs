using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Application.DatabaseAbstraction;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Application.Commands.Employee.Create;

public class CreateHandler : ICommandHandler<Guid, CreateCommand>
{
    private readonly ILogger<CreateHandler> _logger;
    private readonly IEmployeeWriteRepository _employeeWriteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateHandler(
        ILogger<CreateHandler> logger,
        IEmployeeWriteRepository employeeWriteRepository,
        [FromKeyedServices(ModuleKey.Team)] IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _employeeWriteRepository = employeeWriteRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<Guid, ErrorList>> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var employeeId = EmployeeId.CreateNewId();
        
        var employee = new Domain.Entities.Employee(employeeId);
        
        await _employeeWriteRepository.AddEmployeeAsync(employee, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();

        return employeeId.Value;
    }
}