using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.Performances.Application.DatabaseAbstraction;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Performances.Application.Queries.GroupOfSkills.GetEmployeeManagerGradeForGroup;

public class GetEmployeeManagerGradeForGroupHandler : IQueryHandler<List<string>, GetEmployeeManagerGradeForGroupQuery>
{
    private readonly ILogger<GetEmployeeManagerGradeForGroupHandler> _logger;
    private readonly IReadDbContext _readDbContext;

    public GetEmployeeManagerGradeForGroupHandler(ILogger<GetEmployeeManagerGradeForGroupHandler> logger,
        IReadDbContext readDbContext)
    {
        _logger = logger;
        _readDbContext = readDbContext;
    }

    public async Task<Result<List<string>, ErrorList>> HandleAsync(GetEmployeeManagerGradeForGroupQuery query,
        CancellationToken cancellationToken)
    {
        var skillIds = await _readDbContext.GroupSkills
            .Where(g => g.GroupId == query.GroupId)
            .Select(g => g.SkillId)
            .ToListAsync(cancellationToken);

        var records = await _readDbContext.RecordSkills
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