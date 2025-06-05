using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.Core.Validators;
using TeamPulse.Performances.Contract;
using TeamPulse.Performances.Contract.Dtos;
using TeamPulse.Performances.Contract.Requests.SkillGrade;
using TeamPulse.Reports.Application.DatabaseAbstractions.Repositories;
using TeamPulse.Reports.Application.Factories;
using TeamPulse.Reports.Contract.Dtos;
using TeamPulse.Reports.Domain.Enums;
using TeamPulse.Reports.Domain.Reports;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Contract;

namespace TeamPulse.Reports.Application.Commands.GenerateMedianValueReport;

public class GenerateMedianValueHandler : ICommandHandler<BaseReport, GenerateMedianValueCommand>
{
    private readonly ILogger<GenerateMedianValueHandler> _logger;
    private readonly IValidator<GenerateMedianValueCommand> _validator;
    private readonly ITeamContract _teamContract;
    private readonly IPerformanceContract _performanceContract;

    public GenerateMedianValueHandler(
        ILogger<GenerateMedianValueHandler> logger,
        IValidator<GenerateMedianValueCommand> validator,
        ITeamContract teamContract,
        IPerformanceContract performanceContract)
    {
        _logger = logger;
        _validator = validator;
        _teamContract = teamContract;
        _performanceContract = performanceContract;
    }

    public async Task<Result<BaseReport, ErrorList>> HandleAsync(GenerateMedianValueCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var getGroupResult = await _performanceContract.GetGroupOfSkillsByIdAsync(command.Subject, cancellationToken);
        if (getGroupResult.IsFailure)
            return getGroupResult.Error;

        var group = getGroupResult.Value;

        return command.ObjectType switch
        {
            ObjectType.Employee => await HandleEmployeeAsync(command, group, cancellationToken),
            ObjectType.Team => await HandleTeamAsync(command, group, cancellationToken),
            ObjectType.Department => await HandleDepartmentAsync(command, group, cancellationToken),
            _ => Errors.General.ValueIsInvalid($"Unknown report object type: {command.ObjectType}").ToErrorList()
        };
    }

    private async Task<Result<BaseReport, ErrorList>> HandleEmployeeAsync(GenerateMedianValueCommand command,
        GroupOfSkillsDto group, CancellationToken cancellationToken)
    {
        var baseInfoResult =
            await GetEmployeeBaseReport(command.Object, command.Name, command.Description, cancellationToken);
        if (baseInfoResult.IsFailure)
            return baseInfoResult.Error;

        var sourceResult = await _performanceContract.GetEmployeeManagerGradeForGroupAsync(
            new GetEmployeeManagerGradeForGroupRequest(command.Object, group.Id), cancellationToken);
        if (sourceResult.IsFailure)
            return sourceResult.Error;

        return GenerateReportByType(group.SkillGrade.GradeType, command.Object, sourceResult.Value,
            baseInfoResult.Value);
    }

    private async Task<Result<BaseReport, ErrorList>> HandleTeamAsync(GenerateMedianValueCommand command,
        GroupOfSkillsDto group, CancellationToken cancellationToken)
    {
        var baseInfoResult =
            await GetTeamBaseReport(command.Object, command.Name, command.Description, cancellationToken);
        if (baseInfoResult.IsFailure)
            return baseInfoResult.Error;

        var getEmployeesInTeamResult =
            await _teamContract.GetAllEmployeesFromTeamAsync(command.Object, cancellationToken);
        if (getEmployeesInTeamResult.IsFailure)
            return getEmployeesInTeamResult.Error;
        var employees = getEmployeesInTeamResult.Value;

        var sourceResult = await _performanceContract.GetTeamManagerGradeForGroupAsync(
            new GetTeamManagerGradeForGroupRequest(employees, group.Id), cancellationToken);
        if (sourceResult.IsFailure)
            return sourceResult.Error;

        return GenerateReportByType(group.SkillGrade.GradeType, command.Object, sourceResult.Value,
            baseInfoResult.Value);
    }

    private async Task<Result<BaseReport, ErrorList>> HandleDepartmentAsync(GenerateMedianValueCommand command,
        GroupOfSkillsDto group, CancellationToken cancellationToken)
    {
        var baseInfoResult =
            await GetDepartmentBaseReport(command.Object, command.Name, command.Description, cancellationToken);
        if (baseInfoResult.IsFailure)
            return baseInfoResult.Error;

        var getTeamInDepartmentResult = await _teamContract.GetAllTeamsFromDepartmentAsync(command.Object, cancellationToken);
        if (getTeamInDepartmentResult.IsFailure)
            return getTeamInDepartmentResult.Error;
        
        var teamIds = getTeamInDepartmentResult.Value;
        
        var sourceResult = await _performanceContract.GetDepartmentManagerGradeForGroupAsync(
            new GetDepartmentManagerGradeForGroupRequest(teamIds, group.Id), cancellationToken);
        if (sourceResult.IsFailure)
            return sourceResult.Error;

        return GenerateReportByType(group.SkillGrade.GradeType, command.Object, sourceResult.Value,
            baseInfoResult.Value);
    }

    private static Result<BaseReport, ErrorList> GenerateReportByType(string gradeType, Guid objectId,
        IEnumerable<string> rawGrades, InfoToGenerateReportDto baseInfo)
    {
        return gradeType switch
        {
            "numeric_grade" => TryGenerate(rawGrades.Select(int.Parse), objectId, baseInfo),
            "symbol_grade" => TryGenerate(rawGrades, objectId, baseInfo),
            _ => Errors.General.ValueIsInvalid("Unknown skill grade type").ToErrorList()
        };
    }

    private static Result<BaseReport, ErrorList> TryGenerate<T>(IEnumerable<T> grades, Guid objectId,
        InfoToGenerateReportDto info)
    {
        var result = GradeReportFactory<T>.GenerateReport(objectId, grades, info);
        return result.IsSuccess ? result.Value : result.Error.ToErrorList();
    }

    private async Task<Result<InfoToGenerateReportDto, ErrorList>> GetEmployeeBaseReport(Guid id, string name,
        string description, CancellationToken token)
    {
        var result = await _teamContract.GetEmployeeByIdAsync(id, token);
        if (result.IsFailure)
            return result.Error;

        var e = result.Value;
        return new InfoToGenerateReportDto(e.DepartmentId!.Value, e.TeamId, e.Id, description, name, ReportType.MedianValue);
    }

    private async Task<Result<InfoToGenerateReportDto, ErrorList>> GetTeamBaseReport(Guid id, string name,
        string description, CancellationToken token)
    {
        var result = await _teamContract.GetTeamByIdAsync(id, token);
        if (result.IsFailure) return result.Error;

        var t = result.Value;
        return new InfoToGenerateReportDto(t.DepartmentId, t.Id, null, description, name, ReportType.MedianValue);
    }

    private async Task<Result<InfoToGenerateReportDto, ErrorList>> GetDepartmentBaseReport(Guid id, string name,
        string description, CancellationToken token)
    {
        var result = await _teamContract.GetDepartmentByIdAsync(id, token);
        if (result.IsFailure) return result.Error;

        var d = result.Value;
        return new InfoToGenerateReportDto(d.Id, null, null, description, name, ReportType.MedianValue);
    }
}