using System.Text.Json;

namespace TeamPulse.Performances.Contract.Requests.SkillGrade;

public record CreateRequest(
    List<JsonElement> Grades,
    string Name,
    string Description);