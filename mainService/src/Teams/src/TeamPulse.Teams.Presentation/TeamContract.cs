using CSharpFunctionalExtensions;
using TeamPulse.Core.Abstractions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Application.Commands.Department.Create;
using TeamPulse.Teams.Application.Commands.Team.Create;
using TeamPulse.Teams.Application.Queries.Department;
using TeamPulse.Teams.Application.Queries.Department.GetAllTeamsFromDepartment;
using TeamPulse.Teams.Application.Queries.Employee;
using TeamPulse.Teams.Application.Queries.Team;
using TeamPulse.Teams.Application.Queries.Team.GetAllEmployeesFromTeam;
using TeamPulse.Teams.Contract;
using TeamPulse.Teams.Contract.Dtos;
using TeamPulse.Teams.Contract.Requests.Department;
using TeamPulse.Teams.Contract.Requests.Team;

namespace TeamPulse.Team.Presentation;

public class TeamContract : ITeamContract
{
    private readonly ICommandHandler<Guid, CreateDepartmentCommand> _createDepartmentHandler;
    private readonly ICommandHandler<Guid, CreateTeamCommand> _createTeamCommandHandler;
    private readonly IQueryHandler<TeamDto, GetTeamByIdQuery> _getTeamByIdQueryHandler;
    private readonly IQueryHandler<EmployeeDto, GetEmployeeByIdQuery> _getEmployeeQueryHandler;
    private readonly IQueryHandler<DepartmentDto, GetDepartmentByIdQuery> _getDepartmentByIdQueryHandler;
    private readonly IQueryHandler<List<Guid>, GetAllEmployeesFromTeamQuery> _getAllEmployeesFromTeamQueryHandler;
    private readonly IQueryHandler<List<Guid>, GetAllTeamsFromDepartmentQuery> _getAllTeamsFromDepartmentQueryHandler;

    public TeamContract(
        ICommandHandler<Guid, CreateDepartmentCommand> createDepartmentHandler,
        ICommandHandler<Guid, CreateTeamCommand> createTeamCommandHandler,
        IQueryHandler<TeamDto, GetTeamByIdQuery> getTeamByIdQueryHandler,
        IQueryHandler<EmployeeDto, GetEmployeeByIdQuery> getEmployeeQueryHandler,
        IQueryHandler<DepartmentDto, GetDepartmentByIdQuery> getDepartmentByIdQueryHandler,
        IQueryHandler<List<Guid>, GetAllEmployeesFromTeamQuery> getAllEmployeesFromTeamQueryHandler,
        IQueryHandler<List<Guid>, GetAllTeamsFromDepartmentQuery> getAllTeamsFromDepartmentQueryHandler)
    {
        _createDepartmentHandler = createDepartmentHandler;
        _createTeamCommandHandler = createTeamCommandHandler;
        _getTeamByIdQueryHandler = getTeamByIdQueryHandler;
        _getEmployeeQueryHandler = getEmployeeQueryHandler;
        _getDepartmentByIdQueryHandler = getDepartmentByIdQueryHandler;
        _getAllEmployeesFromTeamQueryHandler = getAllEmployeesFromTeamQueryHandler;
        _getAllTeamsFromDepartmentQueryHandler = getAllTeamsFromDepartmentQueryHandler;
    }

    public async Task<Result<Guid, ErrorList>> CreateDepartmentAsync(CreateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateDepartmentCommand(request.Name, request.Teams, request.HeadOfDepartment);

        var result = await _createDepartmentHandler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error;

        return result.Value;
    }

    public async Task<Result<DepartmentDto, ErrorList>> GetDepartmentByIdAsync(Guid departmentId, CancellationToken cancellationToken)
    {
        var query = new GetDepartmentByIdQuery(departmentId);
        
        var result = await _getDepartmentByIdQueryHandler.HandleAsync(query, cancellationToken);
        if (result.IsFailure)
            return result.Error;
        
        return result.Value;
    }


    public async Task<Result<EmployeeDto, ErrorList>> GetEmployeeByIdAsync(Guid employeeId, CancellationToken cancellationToken)
    {
        var query = new GetEmployeeByIdQuery(employeeId);
        
        var result = await _getEmployeeQueryHandler.HandleAsync(query, cancellationToken);
        if (result.IsFailure)
            return result.Error;
        
        return result.Value;
    }

    public async Task<Result<List<Guid>, ErrorList>> GetAllEmployeesFromTeamAsync(Guid teamId, CancellationToken cancellationToken)
    {
        var query = new GetAllEmployeesFromTeamQuery(teamId);
        
        var result = await _getAllEmployeesFromTeamQueryHandler.HandleAsync(query, cancellationToken);
        if (result.IsFailure)
            return result.Error;
        
        return result.Value;
    }

    public async Task<Result<List<Guid>, ErrorList>> GetAllTeamsFromDepartmentAsync(Guid departmentId, CancellationToken cancellationToken)
    {
        var query = new GetAllTeamsFromDepartmentQuery(departmentId);
        
        var result = await _getAllTeamsFromDepartmentQueryHandler.HandleAsync(query, cancellationToken);
        if (result.IsFailure)
            return result.Error;
        
        return result.Value;
    }

    public async Task<Result<TeamDto, ErrorList>> GetTeamByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetTeamByIdQuery(id);

        var result = await _getTeamByIdQueryHandler.HandleAsync(query, cancellationToken);
        if (result.IsFailure)
            return result.Error;
        
        return result.Value;
    }

    public async Task<Result<Guid, ErrorList>> CreateTeamAsync(CreateTeamRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateTeamCommand(
            request.Name,
            request.DepartmentId,
            request.Employees,
            request.HeadOfTeam);
        
        var result = await _createTeamCommandHandler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error;
        
        return result.Value;
    }
}