using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.Performances.Application.DatabaseAbstraction;
using TeamPulse.Performances.Contract.Dtos;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Performances.Application.Queries.GroupOfSkills.GetGroupById;

public class GetGroupByIdHandler : IQueryHandler<GroupOfSkillsDto, GetGroupByIdQuery>
{
    private readonly ILogger<GetGroupByIdHandler> _logger;
    private readonly IReadDbContext _readDbContext;

    public GetGroupByIdHandler(ILogger<GetGroupByIdHandler> logger, IReadDbContext readDbContext)
    {
        _logger = logger;
        _readDbContext = readDbContext;
    }
    public async Task<Result<GroupOfSkillsDto, ErrorList>> HandleAsync(GetGroupByIdQuery query, CancellationToken cancellationToken)
    {
        var group = await _readDbContext.GroupOfSkills
            .Include(g => g.SkillGrade)
            .FirstOrDefaultAsync(g => g.Id == query.GroupId, cancellationToken);

        if (group is null)
        {
            var errorMessage = $"Group {query.GroupId} not found";
            _logger.LogError(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }

        return group;
    }
}