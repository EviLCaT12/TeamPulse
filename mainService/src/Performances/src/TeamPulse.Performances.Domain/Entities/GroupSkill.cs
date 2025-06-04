using TeamPulse.Performances.Domain.ValueObjects.Ids;

namespace TeamPulse.Performances.Domain.Entities;

/// <summary>
/// Таблица сопоставляет группу и ей принадлежащий скилл
/// </summary>
public class GroupSkill
{
    //ef core
    private GroupSkill() { }

    public GroupSkill(
        GroupOfSkillsId groupId, 
        GroupOfSkills groupSkill,
        SkillId skillId,
        Skill skill)
    {
        GroupId = groupId;
        GroupOfSkills = groupSkill;
        SkillId = skillId;
        Skill = skill;
    }
    
    public GroupOfSkillsId GroupId { get; private set; }
    
    public GroupOfSkills GroupOfSkills { get; private set; }
    public SkillId SkillId { get; private set; }
    
    public Skill Skill { get; private set; }
}