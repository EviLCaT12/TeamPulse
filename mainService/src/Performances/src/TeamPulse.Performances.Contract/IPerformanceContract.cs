using CSharpFunctionalExtensions;
using TeamPulse.Performances.Contract.Dtos;
using TeamPulse.Performances.Contract.Requests.SkillGrade;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Performances.Contract;

public interface IPerformanceContract
{
    Task<Result<GroupOfSkillsDto, ErrorList>> GetGroupOfSkillsByIdAsync(Guid groupId,
        CancellationToken cancellationToken);
    
    Task<Result<IEnumerable<string>, ErrorList>> GetEmployeeManagerGradeForGroupAsync(
        GetEmployeeManagerGradeForGroupRequest request, CancellationToken cancellationToken);
    
    Task<Result<IEnumerable<string>, ErrorList>> GetTeamManagerGradeForGroupAsync(
        GetTeamManagerGradeForGroupRequest request, CancellationToken cancellationToken);
    
    Task<Result<IEnumerable<string>, ErrorList>> GetDepartmentManagerGradeForGroupAsync(
        GetDepartmentManagerGradeForGroupRequest request, CancellationToken cancellationToken);
}