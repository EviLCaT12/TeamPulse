using System.Text.Json;

namespace TeamPulse.Performances.Contract.Requests.SkillGrade;

public record CreateGradeRequest(
    List<JsonElement> Grades,
    string Name,
    string Description);