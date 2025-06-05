using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Contract.Dtos;
using TeamPulse.Teams.Contract.Requests.Department;

namespace TeamPulse.Teams.Contract;

public interface ITeamContract
{
    Task<Result<Guid, ErrorList>> CreateDepartmentAsync(CreateDepartmentRequest request,
        CancellationToken cancellationToken);

    Task<Result<DepartmentDto, ErrorList>> GetDepartmentByIdAsync(Guid departmentId,
        CancellationToken cancellationToken);
    
    Task<Result<EmployeeDto, ErrorList>> GetEmployeeByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<Result<List<Guid>, ErrorList>> GetAllEmployeesFromTeamAsync(Guid teamId, CancellationToken cancellationToken);
    
    Task<Result<List<Guid>, ErrorList>> GetAllTeamsFromDepartmentAsync(Guid departmentId, CancellationToken cancellationToken);
    
    Task<Result<TeamDto, ErrorList>> GetTeamByIdAsync(Guid id, CancellationToken cancellationToken);
}