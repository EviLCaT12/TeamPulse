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
    private readonly ITeamRepository _teamRepository;
    private readonly IValidator<DeleteCommand> _validator;

    public DeleteHandler(
        ILogger<DeleteHandler> logger,
        [FromKeyedServices(ModuleKey.Team)] IUnitOfWork unitOfWork,
        ITeamRepository teamRepository,
        IValidator<DeleteCommand> validator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _teamRepository = teamRepository;
        _validator = validator;
    }
    public async Task<UnitResult<ErrorList>> HandleAsync(DeleteCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var teamId = TeamId.Create(command.TeamId).Value;
        var team = await _teamRepository.GetTeamIdAsync(teamId, cancellationToken);
        if (team is null)
        {
            var errorMessage = $"Team {command.TeamId} was not found.";
            _logger.LogError(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }

        if (team.HeadOfTeam is not null)
        {
            _logger.LogInformation($"Deleting team with id {teamId.Value} has head of team {team.HeadOfTeam.Id}.");
        }
        
        _teamRepository.DeleteTeam(team);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();

        return UnitResult.Success<ErrorList>();
    }
}