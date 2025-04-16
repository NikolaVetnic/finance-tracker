namespace Support.Cqrs.Abstractions;

public interface IMediator
{
    Task<TResult?> SendAsync<TRequest, TResult>(TRequest request);
}