using TeamPulse.Core.Abstractions;

namespace TeamPulse.Performances.Application.Commands.Skill.Create;

public record CreateCommand(
    Guid GradeId,
    string Name,
    string Description) : ICommand;