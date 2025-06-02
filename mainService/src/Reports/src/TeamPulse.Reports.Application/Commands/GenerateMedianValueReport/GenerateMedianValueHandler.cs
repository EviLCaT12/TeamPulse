// using CSharpFunctionalExtensions;
// using FluentValidation;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Logging;
// using TeamPulse.Core.Abstractions;
// using TeamPulse.Core.Validators;
// using TeamPulse.Performances.Contract;
// using TeamPulse.Reports.Application.DatabaseAbstractions.Repositories;
// using TeamPulse.Reports.Application.Factories;
// using TeamPulse.Reports.Domain.Enums;
// using TeamPulse.Reports.Domain.Reports;
// using TeamPulse.SharedKernel.Errors;
// using TeamPulse.Teams.Contract;
//
// namespace TeamPulse.Reports.Application.Commands.GenerateMedianValueReport;
//
// public class GenerateMedianValueHandler : ICommandHandler<BaseReport , GenerateMedianValueCommand>
// {
//     private readonly ILogger<GenerateMedianValueHandler> _logger;
//     private readonly IValidator<GenerateMedianValueCommand> _validator;
//     private readonly ITeamContract _teamContract;
//     private readonly IPerformanceContract _performanceContract;
//     private readonly IReportRepository _reportRepository;
//     private readonly IUnitOfWork _unitOfWork;
//
//     public GenerateMedianValueHandler(
//         ILogger<GenerateMedianValueHandler> logger,
//         IValidator<GenerateMedianValueCommand> validator,
//         ITeamContract teamContract,
//         IPerformanceContract performanceContract,
//         IReportRepository reportRepository,
//         [FromKeyedServices(ModuleKey.Performance)] IUnitOfWork unitOfWork) //ToDo: Заменить по готовности
//     {
//         _logger = logger;
//         _validator = validator;
//         _teamContract = teamContract;
//         _performanceContract = performanceContract;
//         _reportRepository = reportRepository;
//         _unitOfWork = unitOfWork;
//     }
//     public async Task<Result<BaseReport, ErrorList>> HandleAsync(GenerateMedianValueCommand command, CancellationToken cancellationToken)
//     {
//         var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
//         
//         var validationResult = await _validator.ValidateAsync(command, cancellationToken);
//         if (validationResult.IsValid == false)
//             return validationResult.ToErrorList();
//         
//         //Получаю группу скиллов и тип оценки
//         var isGroupExist = await _performanceContract.
//         
//         //Пытаемся угадать, для кого конкретно этот отчет
//         switch (command.ObjectType)
//         {
//             case ObjectType.Employee:
//                 
//         }
//         
//         BaseReport report = GradeReportFactory<>
//     }
// }