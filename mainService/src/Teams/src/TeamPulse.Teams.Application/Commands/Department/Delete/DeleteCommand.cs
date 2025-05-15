using TeamPulse.Core.Abstractions;

namespace TeamPulse.Teams.Application.Commands.Department.Delete;

public record DeleteCommand(Guid DepartmentId) : ICommand;