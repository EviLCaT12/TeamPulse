using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.Performances.Application.DatabaseAbstraction;
using TeamPulse.Performances.Application.DatabaseAbstraction.Repositories.Read;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Performances.Application.Queries.GroupOfSkills.GetTeamManagerGradeForGroup;

public class GetTeamManagerGradeForGroupHandler : IQueryHandler<List<string>, GetTeamManagerGradeForGroupQuery>
{
    private readonly ILogger<GetTeamManagerGradeForGroupHandler> _logger;
    private readonly IRecordSkillReadRepository _readRepository;


    public GetTeamManagerGradeForGroupHandler(ILogger<GetTeamManagerGradeForGroupHandler> logger,
        IRecordSkillReadRepository readRepository)
    {
        _logger = logger;
        _readRepository = readRepository;
    }

    public async Task<Result<List<string>, ErrorList>> HandleAsync(GetTeamManagerGradeForGroupQuery query,
        CancellationToken cancellationToken)
    {
        List<string> grades = [];

        foreach (var id in query.EmployeesIds)
        {
            var record = await _readRepository.GetRecordSkills()
                .FirstOrDefaultAsync(r => r.WhoId == id && r.WhatId == query.GroupId, cancellationToken);

            if (record is null)
            {
                var errorMessage = $"No employee with id {id} found for group {query.GroupId}";
                _logger.LogError(errorMessage);
                return Errors.General.ValueNotFound(errorMessage).ToErrorList();
            }

            if (record.ManagerGrade is null)
            {
                var errorMessage = $"Employee with id {id} has not a manager grade.";
                _logger.LogError(errorMessage);
                return Errors.General.ValueIsInvalid(errorMessage).ToErrorList();
            }
            
            grades.Add(record.ManagerGrade);
        }

        return grades;
    }
}