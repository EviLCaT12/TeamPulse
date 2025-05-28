using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Contract.Dtos;
using TeamPulse.Teams.Contract.Requests.Department;

namespace TeamPulse.Teams.Contract;

public interface ITeamContract
{
    Task<Result<Guid, ErrorList>> CreateDepartmentAsync(CreateDepartmentRequest request,
        CancellationToken cancellationToken);
    
    Task<Result<EmployeeDto, ErrorList>> GetEmployeeByIdAsync(Guid id, CancellationToken cancellationToken);
}