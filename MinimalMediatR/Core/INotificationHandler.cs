namespace MinimalMediatR.Core;

public interface INotificationHandler<TNotification> where TNotification : INotification
{
    Task Handle(TNotification notification, CancellationToken cancellationToken);
}