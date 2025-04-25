using MinimalMediatR.Core;

namespace MinimalMediatR.ExampleApp.Features.Users;

public class UserCreatedNotificationHandler : INotificationHandler<UserCreatedNotification>
{
    public async Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[Notification] User created with Id: {notification.UserId}");
        await Task.CompletedTask;
    }
}