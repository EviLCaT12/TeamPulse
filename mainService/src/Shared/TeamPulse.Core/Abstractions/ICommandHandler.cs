using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Core.Abstractions;

public interface ICommandHandler<TResponse, in TCommand> where TCommand : ICommand
{
    Task<Result<TResponse, ErrorList>> HandleAsync(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task<UnitResult<ErrorList>> HandleAsync(TCommand command, CancellationToken cancellationToken);
}
