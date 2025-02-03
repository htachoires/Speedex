namespace Speedex.Domain.Commons;

public interface ICommandHandler<in TCommand, TCommandResult>
    where TCommand : ICommand
    where TCommandResult : ICommandResult
{
    Task<TCommandResult> Handle(TCommand command, CancellationToken cancellationToken = default);
}

public interface ICommand
{
}

public interface ICommandResult
{
}