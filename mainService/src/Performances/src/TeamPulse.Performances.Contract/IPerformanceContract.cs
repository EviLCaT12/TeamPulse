using CSharpFunctionalExtensions;
using TeamPulse.Performances.Contract.Dtos;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Performances.Contract;

public interface IPerformanceContract
{
    Task<Result<GroupOfSkillsDto, ErrorList>> GetGroupOfSkillsByIdAsync(Guid groupId,
        CancellationToken cancellationToken);
}