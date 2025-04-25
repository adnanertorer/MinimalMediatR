using Microsoft.Extensions.DependencyInjection;

namespace MinimalMediatR.Core;

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    public async Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
    {
        ArgumentNullException.ThrowIfNull(request);

        using var scope = serviceProvider.CreateScope(); 
        var scopedProvider = scope.ServiceProvider;

        var handler = GetHandler<TRequest, TResponse>(scopedProvider);
        var behaviors = GetBehaviors<TRequest, TResponse>(scopedProvider);

        Func<Task<TResponse>> handlerInvocation = () => handler.Handle(request, cancellationToken);

        foreach (var behavior in behaviors.Reverse())
        {
            var next = handlerInvocation;
            handlerInvocation = () => behavior.Handle(request, cancellationToken, next);
        }

        return await handlerInvocation();
    }

    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        ArgumentNullException.ThrowIfNull(notification);

        using var scope = serviceProvider.CreateScope(); 
        var scopedProvider = scope.ServiceProvider;

        var handlers = scopedProvider.GetServices<INotificationHandler<TNotification>>();

        var tasks = handlers.Select(handler => handler.Handle(notification, cancellationToken));
        await Task.WhenAll(tasks);
    }

    private IRequestHandler<TRequest, TResponse> GetHandler<TRequest, TResponse>(IServiceProvider scopedProvider)
       where TRequest : IRequest<TResponse>
    {
        var handler = scopedProvider.GetService<IRequestHandler<TRequest, TResponse>>();

        if (handler == null)
        {
            throw new InvalidOperationException($"No handler found for {typeof(TRequest).Name}");
        }

        return handler;
    }

    private IEnumerable<IPipelineBehavior<TRequest, TResponse>> GetBehaviors<TRequest, TResponse>(IServiceProvider scopedProvider)
        where TRequest : IRequest<TResponse>
    {
        return scopedProvider.GetServices<IPipelineBehavior<TRequest, TResponse>>();
    }
}