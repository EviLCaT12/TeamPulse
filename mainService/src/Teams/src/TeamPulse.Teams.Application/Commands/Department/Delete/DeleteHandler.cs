using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.Core.Validators;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Application.DatabaseAbstraction;
using TeamPulse.Teams.Application.DatabaseAbstraction.Repositories.Write;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Application.Commands.Department.Delete;

public class DeleteHandler : ICommandHandler<DeleteCommand>
{
    private readonly ILogger<DeleteHandler> _logger;
    private readonly IValidator<DeleteCommand> _validator;
    private readonly IDepartmentWriteRepository _departmentWriteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteHandler(
        ILogger<DeleteHandler> logger, 
        IValidator<DeleteCommand> validator,
        IDepartmentWriteRepository departmentWriteRepository,
        [FromKeyedServices(ModuleKey.Team)] IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _validator = validator;
        _departmentWriteRepository = departmentWriteRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UnitResult<ErrorList>> HandleAsync(DeleteCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var departmentId = DepartmentId.Create(command.DepartmentId).Value;
        var department = await _departmentWriteRepository.GetDepartmentByIdAsync(departmentId, cancellationToken);
        if (department is null)
        {
            var errorMessage = $"Department with id {departmentId} was not found.";
            _logger.LogError(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }

        if (department.Teams.Any())
        {
            _logger.LogInformation($"Deleting department {departmentId} has teams.");
        }

        if (department.HeadOfDepartment is not null)
        {
            _logger.LogInformation($"Deleting department {departmentId} has head.");
        }
        
        _departmentWriteRepository.DeleteDepartment(department);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();

        return UnitResult.Success<ErrorList>();
    }
}