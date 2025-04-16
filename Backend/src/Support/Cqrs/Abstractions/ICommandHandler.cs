namespace Support.Cqrs.Abstractions;

public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<TResult?> HandleAsync(TCommand command);
}