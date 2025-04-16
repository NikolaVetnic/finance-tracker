using Microsoft.Extensions.DependencyInjection;
using Support.Cqrs.Abstractions;

namespace Support.Cqrs;

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    public async Task<TResult?> SendAsync<TRequest, TResult>(TRequest request)
    {
        var behaviors = serviceProvider.GetServices<IPipelineBehavior<TRequest, TResult>>();

        // Chain the behaviors with the main request handling
        RequestHandlerDelegate<TResult> next = () => HandleRequest<TRequest, TResult>(request);

        // Apply each behavior in reverse order so the first registered behavior runs first
        foreach (var behavior in behaviors.Reverse())
        {
            var currentNext = next;
            next = () => behavior.Handle(request, CancellationToken.None, currentNext);
        }

        return await next();
    }
    
    private async Task<TResult> HandleRequest<TRequest, TResult>(TRequest request)
    {
        switch (request)
        {
            case ICommand<TResult> commandRequest:
            {
                var handlerType = typeof(ICommandHandler<,>).MakeGenericType(request.GetType(), typeof(TResult));
                dynamic handler = serviceProvider.GetService(handlerType) ?? 
                                  throw new InvalidOperationException("Handler not found for command.");
            
                return await handler.HandleAsync((dynamic)commandRequest);
            }
            case IQuery<TResult> queryRequest:
            {
                var handlerType = typeof(IQueryHandler<,>).MakeGenericType(request.GetType(), typeof(TResult));
                dynamic handler = serviceProvider.GetService(handlerType) ?? 
                                  throw new InvalidOperationException("Handler not found for query.");
            
                return await handler.HandleAsync((dynamic)queryRequest);
            }
            default:
                throw new ArgumentException("Request type does not match any command or query type.");
        }
    }
}