using Microsoft.Extensions.DependencyInjection;

namespace MinimalMediatR.Core;

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    public async Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
    {
        ArgumentNullException.ThrowIfNull(request);

        var handler = GetHandler<TRequest, TResponse>();
        var behaviors = GetBehaviors<TRequest, TResponse>();

        Func<Task<TResponse>> handlerInvocation = () => handler.Handle(request, cancellationToken);

        foreach (var behavior in behaviors.Reverse())
        {
            var next = handlerInvocation;
            handlerInvocation = () => behavior.Handle(request, cancellationToken, next);
        }

        return await handlerInvocation();
    }

    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
    {
        ArgumentNullException.ThrowIfNull(notification);

        var handlers = serviceProvider.GetServices<INotificationHandler<TNotification>>();

        var tasks = handlers.Select(handler => handler.Handle(notification, cancellationToken));
        await Task.WhenAll(tasks);
    }

    private IRequestHandler<TRequest, TResponse> GetHandler<TRequest, TResponse>()
        where TRequest : IRequest<TResponse>
    {
        var handler = serviceProvider.GetService<IRequestHandler<TRequest, TResponse>>();

        if (handler == null)
        {
            throw new InvalidOperationException($"No handler found for {typeof(TRequest).Name}");
        }

        return handler;
    }

    private IEnumerable<IPipelineBehavior<TRequest, TResponse>> GetBehaviors<TRequest, TResponse>()
        where TRequest : IRequest<TResponse>
    {
        return serviceProvider.GetServices<IPipelineBehavior<TRequest, TResponse>>();
    }
}