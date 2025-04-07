namespace MinimalMediatR.Core;

public interface IMediator
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken);

    Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken)
        where TNotification : INotification;
}