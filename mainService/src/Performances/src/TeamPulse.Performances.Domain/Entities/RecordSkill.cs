using TeamPulse.Performances.Domain.Entities.SkillGrade;
using TeamPulse.Performances.Domain.ValueObjects.Ids;

namespace TeamPulse.Performances.Domain.Entities;

/// <summary>
/// Данная сущность предназначена для хранения информации о том,
/// какая команда или сотрудник владеет данным скилом, его оценке
/// и оценке главного
/// </summary>
public class RecordSkill
{
    //ef core
    private RecordSkill() {}
    
    //ToDo: заменить на нормальный метод Create
    public RecordSkill(
        Guid whoId,
        Guid whatId,
        string? selfGrade,
        string? managerGrade)
    {
        WhoId = whoId;
        WhatId = whatId;
        SelfGrade = selfGrade ?? SelfGrade;
        ManagerGrade = managerGrade ?? ManagerGrade;
    }
    //Либо отдел, либо команда, либо сотрудник - всё равно.
    public Guid WhoId { get; private set; }
    
    //Группа скиллов или одиночный скилл
    public Guid WhatId { get; private set; }

    public string? SelfGrade { get; private set; } = null;
    
    public string? ManagerGrade { get; private set; } = null;
 }