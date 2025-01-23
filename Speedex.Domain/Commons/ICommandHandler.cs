namespace Speedex.Domain.Commons;

public interface ICommandHandler<in TCommand, out TCommandResult>
    where TCommand : ICommand
    where TCommandResult : ICommandResult
{
    TCommandResult Handle(TCommand command);
}

public interface ICommand
{
}

public interface ICommandResult
{
}