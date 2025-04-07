using MinimalMediatR.Core;

namespace MinimalMediatR.Tests.Events;

public class UserCreatedEvent : INotification
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
}