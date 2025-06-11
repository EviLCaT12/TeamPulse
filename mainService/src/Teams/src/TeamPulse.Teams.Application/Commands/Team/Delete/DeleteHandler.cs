using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.Core.Validators;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Application.DatabaseAbstraction;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Application.Commands.Team.Delete;

public class DeleteHandler : ICommandHandler<DeleteCommand>
{
    private readonly ILogger<DeleteHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITeamWriteRepository _teamWriteRepository;
    private readonly IValidator<DeleteCommand> _validator;

    public DeleteHandler(
        ILogger<DeleteHandler> logger,
        [FromKeyedServices(ModuleKey.Team)] IUnitOfWork unitOfWork,
        ITeamWriteRepository teamWriteRepository,
        IValidator<DeleteCommand> validator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _teamWriteRepository = teamWriteRepository;
        _validator = validator;
    }
    public async Task<UnitResult<ErrorList>> HandleAsync(DeleteCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var teamId = TeamId.Create(command.TeamId).Value;
        var team = await _teamWriteRepository.GetTeamByIdAsync(teamId, cancellationToken);
        if (team is null)
        {
            var errorMessage = $"Team {command.TeamId} was not found.";
            _logger.LogError(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }
        
        _teamWriteRepository.DeleteTeam(team);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();

        return UnitResult.Success<ErrorList>();
    }
}