using System.Text.Json;
using TeamPulse.Core.Abstractions;

namespace TeamPulse.Performances.Application.Commands.SkillGrade.Create;

public record CreateCommand(
    List<JsonElement> Grades,
    string Name,
    string Description) : ICommand;