using TeamPulse.Core.Abstractions;

namespace TeamPulse.Performances.Application.Commands.GroupOfSkills.Create;

public record CreateCommand(
    string Name, 
    string Description,
    Guid GradeId) : ICommand;