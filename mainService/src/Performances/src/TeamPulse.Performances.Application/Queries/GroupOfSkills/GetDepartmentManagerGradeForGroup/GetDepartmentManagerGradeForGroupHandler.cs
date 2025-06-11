using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.Performances.Application.DatabaseAbstraction;
using TeamPulse.Performances.Application.DatabaseAbstraction.Repositories.Read;
using TeamPulse.Performances.Contract.Dtos;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Performances.Application.Queries.GroupOfSkills.GetDepartmentManagerGradeForGroup;

public class
    GetDepartmentManagerGradeForGroupHandler : IQueryHandler<List<string>, GetDepartmentManagerGradeForGroupQuery>
{
    private readonly ILogger<GetDepartmentManagerGradeForGroupHandler> _logger;
    private readonly IRecordSkillReadRepository _readRepository;


    public GetDepartmentManagerGradeForGroupHandler(ILogger<GetDepartmentManagerGradeForGroupHandler> logger,
       IRecordSkillReadRepository readRepository)
    {
        _logger = logger;
        _readRepository = readRepository;
    }

    public async Task<Result<List<string>, ErrorList>> HandleAsync(GetDepartmentManagerGradeForGroupQuery query,
        CancellationToken cancellationToken)
    {
        var recordWithGroup = await GetAllRecordsForGroup(query.GroupId, cancellationToken);
        if (recordWithGroup.IsFailure)
            return recordWithGroup.Error;
        
        var grades = GetAllGrades(recordWithGroup.Value, query.TeamIds);
        if (grades.IsFailure)
            return grades.Error;

        return grades.Value;
    }

    private Result<List<string>, ErrorList> GetAllGrades(List<RecordSkillDto> records, IEnumerable<Guid> teamIds)
    {
        //Сугубо для логгирования ошибки
        var group = records.First().WhatId;

        List<string> grades = [];
        
        foreach (var id in teamIds)
        {
            var record = records.FirstOrDefault(r => r.WhoId == id);

            if (record is null)
            {
                var errorMessage = $"There is no record for team with id {id} with group {group}.";
                _logger.LogError(errorMessage);
                return Errors.General.ValueNotFound(errorMessage).ToErrorList();
            }

            if (string.IsNullOrEmpty(record.ManagerGrade))
            {
                var errorMessage = $"There is no manager grade for team with id {id} with group {group}.";
                _logger.LogError(errorMessage);
                return Errors.General.ValueIsInvalid(errorMessage).ToErrorList();
            }
            
            grades.Add(record.ManagerGrade);
        }

        return grades;
    }

    private async Task<Result<List<RecordSkillDto>, ErrorList>> GetAllRecordsForGroup(Guid groupId,
        CancellationToken cancellationToken)
    {
        var records = await _readRepository
            .GetRecordSkills()
            .Where(r => r.WhatId == groupId)
            .ToListAsync(cancellationToken);
            

        return records;
    }
}