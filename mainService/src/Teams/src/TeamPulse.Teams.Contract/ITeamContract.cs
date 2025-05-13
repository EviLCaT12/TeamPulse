using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Teams.Contract;

public interface ITeamContract
{
    Task<Result<Guid, ErrorList>> CreateDepartmentAsync(CreateDepartmentRequest request,
        CancellationToken cancellationToken);
}