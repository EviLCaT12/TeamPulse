using TeamPulse.Core.Abstractions;

namespace TeamPulse.Performances.Application.Commands.GroupOfSkills.AddSkill;

public record AddSkillToGroupCommand(
    Guid GroupId,
    Guid SkillId) : ICommand;