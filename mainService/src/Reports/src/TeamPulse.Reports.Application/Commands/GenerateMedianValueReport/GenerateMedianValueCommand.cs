using TeamPulse.Core.Abstractions;
using TeamPulse.Reports.Domain.Enums;

namespace TeamPulse.Reports.Application.Commands.GenerateMedianValueReport;

public record GenerateMedianValueCommand(
    string Name,
    string Description,
    Guid Object, //Отдел, команда, сотрудник
    ObjectType ObjectType,
    Guid Subject //Группа скиллов, скилл (На данный момент пока только группа скиллов)
) : ICommand;