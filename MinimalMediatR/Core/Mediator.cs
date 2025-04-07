namespace MinimalMediatR.Core;

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        var requestType = request.GetType();
        var responseType = typeof(TResponse);
        
        //if request type is GetCustomerQuery and if TResponse type is Customer, handler type is IRequestHandler<GetCustomerQuery, Customer>
       var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType,responseType);
       var handler = serviceProvider.GetService(handlerType);

       if (handler == null)
       {
           throw new InvalidOperationException($"No handler found of {request.GetType().Name}");
       }
       
       //Pipeline behaviors
       var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, responseType);
       var behaviors =
           (IEnumerable<object>)serviceProvider.GetService(typeof(IEnumerable<>).MakeGenericType(behaviorType));
       
       // Final handler invocation (last delegate in pipeline chain)
       Task<TResponse> HandlerDelegate() =>
           (Task<TResponse>)handlerType.GetMethod("Handle")!
               .Invoke(handler, new object?[] { request, cancellationToken });
       
       Func<Task<TResponse>> pipeline = HandlerDelegate;
       
       foreach (var behavior in behaviors.Reverse())
       {
           var method = behaviorType.GetMethod("Handle");
           var next = pipeline;
           pipeline = () => (Task<TResponse>)method!.Invoke(behavior, new object[] { request, cancellationToken, next })!;
       }

       return pipeline();
    }

    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken) where TNotification : INotification
    {
        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notification.GetType());
        var enumerableType = typeof(IEnumerable<>).MakeGenericType(handlerType);
        
        var handlers = (IEnumerable<object>)(serviceProvider.GetService(enumerableType)) ?? Enumerable.Empty<object>();

        foreach (var handler in handlers)
        {
            var method = handlerType.GetMethod("Handle");
            var task = (Task)method!.Invoke(handler, new object[] { notification, cancellationToken });
            await task.ConfigureAwait(false);
        }
    }
}