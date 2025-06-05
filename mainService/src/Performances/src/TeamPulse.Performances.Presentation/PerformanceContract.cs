using CSharpFunctionalExtensions;
using TeamPulse.Core.Abstractions;
using TeamPulse.Performances.Application.Queries.GroupOfSkills.GetDepartmentManagerGradeForGroup;
using TeamPulse.Performances.Application.Queries.GroupOfSkills.GetEmployeeManagerGradeForGroup;
using TeamPulse.Performances.Application.Queries.GroupOfSkills.GetGroupById;
using TeamPulse.Performances.Application.Queries.GroupOfSkills.GetTeamManagerGradeForGroup;
using TeamPulse.Performances.Contract;
using TeamPulse.Performances.Contract.Dtos;
using TeamPulse.Performances.Contract.Requests.SkillGrade;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Performances.Presentation;

public class PerformanceContract : IPerformanceContract
{
    private readonly IQueryHandler<GroupOfSkillsDto, GetGroupByIdQuery> _getGroupByIdQueryHandler;

    private readonly IQueryHandler<List<string>, GetEmployeeManagerGradeForGroupQuery>
        _getEmployeeManagerGradeForGroupQueryHandler;

    private readonly IQueryHandler<List<string>, GetTeamManagerGradeForGroupQuery>
        _getTeamManagerGradeForGroupQueryHandler;

    private readonly IQueryHandler<List<string>, GetDepartmentManagerGradeForGroupQuery>
        _getDepartmentManagerGradeForGroupQueryHandler;

    public PerformanceContract(
        IQueryHandler<GroupOfSkillsDto, GetGroupByIdQuery> getGroupByIdQueryHandler,
        IQueryHandler<List<string>, GetEmployeeManagerGradeForGroupQuery>
            getEmployeeManagerGradeForGroupQueryHandler,
        IQueryHandler<List<string>, GetTeamManagerGradeForGroupQuery> getTeamManagerGradeForGroupQueryHandler,
        IQueryHandler<List<string>, GetDepartmentManagerGradeForGroupQuery>
            getDepartmentManagerGradeForGroupQueryHandler)
    {
        _getGroupByIdQueryHandler = getGroupByIdQueryHandler;
        _getEmployeeManagerGradeForGroupQueryHandler = getEmployeeManagerGradeForGroupQueryHandler;
        _getTeamManagerGradeForGroupQueryHandler = getTeamManagerGradeForGroupQueryHandler;
        _getDepartmentManagerGradeForGroupQueryHandler = getDepartmentManagerGradeForGroupQueryHandler;
    }

    public async Task<Result<GroupOfSkillsDto, ErrorList>> GetGroupOfSkillsByIdAsync(Guid groupId,
        CancellationToken cancellationToken)
    {
        var query = new GetGroupByIdQuery(groupId);

        var result = await _getGroupByIdQueryHandler.HandleAsync(query, cancellationToken);
        if (result.IsFailure)
            return result.Error;

        return result.Value;
    }

    public async Task<Result<IEnumerable<string>, ErrorList>> GetEmployeeManagerGradeForGroupAsync(
        GetEmployeeManagerGradeForGroupRequest request, CancellationToken cancellationToken)
    {
        var query = new GetEmployeeManagerGradeForGroupQuery(request.EmployeeId, request.GroupId);

        var result = await _getEmployeeManagerGradeForGroupQueryHandler.HandleAsync(query, cancellationToken);
        if (result.IsFailure)
            return result.Error;

        return result.Value;
    }

    public async Task<Result<IEnumerable<string>, ErrorList>> GetTeamManagerGradeForGroupAsync(
        GetTeamManagerGradeForGroupRequest request, CancellationToken cancellationToken)
    {
        var query = new GetTeamManagerGradeForGroupQuery(request.EmployeeIds, request.GroupId);

        var result = await _getTeamManagerGradeForGroupQueryHandler.HandleAsync(query, cancellationToken);
        if (result.IsFailure)
            return result.Error;

        return result.Value;
    }

    public async Task<Result<IEnumerable<string>, ErrorList>> GetDepartmentManagerGradeForGroupAsync(
        GetDepartmentManagerGradeForGroupRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetDepartmentManagerGradeForGroupQuery(request.TeamIds, request.GroupId);
        
        var result = await _getDepartmentManagerGradeForGroupQueryHandler.HandleAsync(query, cancellationToken);
        if (result.IsFailure)
            return result.Error;
        
        return result.Value;
    }
}