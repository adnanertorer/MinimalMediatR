using Microsoft.Extensions.Logging;
using MinimalMediatR.Core;
using MinimalMediatR.Tests.Events;

namespace MinimalMediatR.Tests.Handlers;

public class UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger) : INotificationHandler<UserCreatedEvent>
{
    public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("UserCreatedEvent handled for: {UserId} - {UserName}", notification.UserId, notification.UserName);
        return Task.CompletedTask;
    }
}