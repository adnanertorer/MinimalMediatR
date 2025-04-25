using MinimalMediatR.Core;

namespace MinimalMediatR.ExampleApp.Features.Users;

public class UserCreatedNotification : INotification
{
    public Guid UserId { get; set; }
}