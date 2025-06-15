using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.Performances.Application.DatabaseAbstraction;
using TeamPulse.Performances.Application.DatabaseAbstraction.Repositories.Read;
using TeamPulse.Performances.Contract.Dtos;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Performances.Application.Queries.GroupOfSkills.GetGroupById;

public class GetGroupByIdHandler : IQueryHandler<GroupOfSkillsDto, GetGroupByIdQuery>
{
    private readonly ILogger<GetGroupByIdHandler> _logger;
    private readonly IGroupOfSkillReadRepository _groupOfSkillReadRepository;


    public GetGroupByIdHandler(ILogger<GetGroupByIdHandler> logger, IGroupOfSkillReadRepository groupOfSkillReadRepository)
    {
        _logger = logger;
        _groupOfSkillReadRepository = groupOfSkillReadRepository;
    }
    public async Task<Result<GroupOfSkillsDto, ErrorList>> HandleAsync(GetGroupByIdQuery query, CancellationToken cancellationToken)
    {
        var group = await _groupOfSkillReadRepository.GetGroupOfSkills()
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