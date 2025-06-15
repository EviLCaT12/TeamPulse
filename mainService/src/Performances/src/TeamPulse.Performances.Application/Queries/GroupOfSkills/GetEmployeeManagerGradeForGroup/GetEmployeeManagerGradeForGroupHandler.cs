using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.Performances.Application.DatabaseAbstraction.Repositories.Read;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Performances.Application.Queries.GroupOfSkills.GetEmployeeManagerGradeForGroup;

public class GetEmployeeManagerGradeForGroupHandler : IQueryHandler<List<string>, GetEmployeeManagerGradeForGroupQuery>
{
    private readonly ILogger<GetEmployeeManagerGradeForGroupHandler> _logger;
    private readonly IRecordSkillReadRepository _recordSkillReadRepository;
    private readonly IGroupSkillReadRepository _groupSkillReadRepository;

    public GetEmployeeManagerGradeForGroupHandler(
        ILogger<GetEmployeeManagerGradeForGroupHandler> logger,
        IRecordSkillReadRepository recordSkillReadRepository,
        IGroupSkillReadRepository groupSkillReadRepository)
    {
        _logger = logger;
        _recordSkillReadRepository = recordSkillReadRepository;
        _groupSkillReadRepository = groupSkillReadRepository;
    }

    public async Task<Result<List<string>, ErrorList>> HandleAsync(GetEmployeeManagerGradeForGroupQuery query,
        CancellationToken cancellationToken)
    {
        var skillIds = await _groupSkillReadRepository.GetGroupSkills()
            .Where(g => g.GroupId == query.GroupId)
            .Select(g => g.SkillId)
            .ToListAsync(cancellationToken);

        var records = await _recordSkillReadRepository.GetRecordSkills()
            .Where(r => r.WhoId == query.EmployeeId && skillIds.Contains(r.WhatId))
            .Select(r => r.ManagerGrade)
            .ToListAsync(cancellationToken);

        if (records.Count == 0)
        {
            var errorMessage = $"No records found for group {query.GroupId}";
            _logger.LogError(errorMessage);
            Errors.General.ValueNotFound(errorMessage);
        }

        return records!;
    }
}