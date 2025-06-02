using CSharpFunctionalExtensions;
using TeamPulse.Core.Abstractions;
using TeamPulse.Performances.Application.Queries.GroupOfSkills.GetGroupById;
using TeamPulse.Performances.Contract;
using TeamPulse.Performances.Contract.Dtos;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Performances.Presentation;

public class PerformanceContract : IPerformanceContract
{
    private readonly IQueryHandler<GroupOfSkillsDto, GetGroupByIdQuery> _getGroupByIdQueryHandler;

    public PerformanceContract(
        IQueryHandler<GroupOfSkillsDto, GetGroupByIdQuery> getGroupByIdQueryHandler)
    {
        _getGroupByIdQueryHandler = getGroupByIdQueryHandler;
    }
    public async Task<Result<GroupOfSkillsDto, ErrorList>> GetGroupOfSkillsByIdAsync(Guid groupId, CancellationToken cancellationToken)
    {
        var query = new GetGroupByIdQuery(groupId);
        
        var result = await _getGroupByIdQueryHandler.HandleAsync(query, cancellationToken);
        if (result.IsFailure)
            return result.Error;

        return result;
    }
}