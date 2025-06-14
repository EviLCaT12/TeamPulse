using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.Core.Validators;
using TeamPulse.Performances.Application.DatabaseAbstraction;
using TeamPulse.Performances.Application.DatabaseAbstraction.Repositories.Write;
using TeamPulse.Performances.Domain.Entities;
using TeamPulse.Performances.Domain.ValueObjects.Ids;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Performances.Application.Commands.GroupOfSkills.AddSkill;

public class AddSkillToGroupHandler : ICommandHandler<AddSkillToGroupCommand>
{
    private readonly ILogger<AddSkillToGroupHandler> _logger;
    private readonly IValidator<AddSkillToGroupCommand> _validator;
    private readonly IGroupSkillWriteRepository _groupSkillWriteRepository;
    private readonly IGroupOfSkillWriteRepository _groupOfSkillRepository;
    private readonly ISkillWriteRepository _skillWriteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddSkillToGroupHandler(
        ILogger<AddSkillToGroupHandler> logger,
        IValidator<AddSkillToGroupCommand> validator,
        IGroupSkillWriteRepository groupSkillWriteRepository,
        IGroupOfSkillWriteRepository groupOfSkillRepository,
        ISkillWriteRepository skillWriteRepository,
        [FromKeyedServices(ModuleKey.Performance)]
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _validator = validator;
        _groupSkillWriteRepository = groupSkillWriteRepository;
        _groupOfSkillRepository = groupOfSkillRepository;
        _skillWriteRepository = skillWriteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UnitResult<ErrorList>> HandleAsync(AddSkillToGroupCommand command,
        CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var groupId = GroupOfSkillsId.Create(command.GroupId).Value;
        var group = await _groupOfSkillRepository.GetByIdAsync(groupId, cancellationToken);
        if (group is null)
        {
            var errorMessage = $"Group {command.GroupId} does not exist.";
            _logger.LogError(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }

        var skillId = SkillId.Create(command.SkillId).Value;
        var skill = await _skillWriteRepository.GetByIdAsync(skillId, cancellationToken);
        if (skill is null)
        {
            var errorMessage = $"Skill {command.SkillId} does not exist.";
            _logger.LogError(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }

        if (group.SkillGrade != skill.SkillGrade)
        {
            var errorMessage = $"Group {command.GroupId} and skill {command.SkillId} does not have the same grade.";
            _logger.LogError(errorMessage);
            return Errors.General.ValueIsInvalid(errorMessage).ToErrorList();
        }

        var skillInGroup = await _groupSkillWriteRepository.GetByIdAsync(groupId, skillId, cancellationToken);
        if (skillInGroup is not null)
        {
            var errorMessage = $"Skill {command.SkillId} already added to group {command.GroupId}.";
            _logger.LogError(errorMessage);
            return Errors.General.ValueIsInvalid(errorMessage).ToErrorList();
        }

        var skillGroupPair = new GroupSkill(groupId, group, skillId, skill);

        await _groupSkillWriteRepository.AddGroupSkillAsync(skillGroupPair, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        transaction.Commit();

        return UnitResult.Success<ErrorList>();
    }
}