using MinimalMediatR.Core;

namespace MinimalMediatR.Extensions;

public static class MediatorExtensions
{
    public static Task<TResponse> Send<TRequest, TResponse>(this IMediator mediator, TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
    {
        return mediator.Send<TRequest, TResponse>(request, cancellationToken);
    }

    public static Task<TResponse> Send<TResponse>(this IMediator mediator, IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        var method = typeof(IMediator).GetMethod(nameof(IMediator.Send))!.MakeGenericMethod(requestType, typeof(TResponse));
        return (Task<TResponse>)method.Invoke(mediator, new object[] { request, cancellationToken })!;
    }
}