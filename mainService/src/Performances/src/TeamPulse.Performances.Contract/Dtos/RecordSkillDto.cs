namespace TeamPulse.Performances.Contract.Dtos;

public class RecordSkillDto
{
    public Guid WhoId { get; init; }
    
    public Guid WhatId { get; init; }

    public string? SelfGrade { get; init; } = null;
    
    public string? ManagerGrade { get; init; } = null;
}