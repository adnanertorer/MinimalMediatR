using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;
using MinimalMediatR.Tests.Events;
using MinimalMediatR.Tests.Handlers;

namespace MinimalMediatR.Tests;

public class NotificationTests
{
    [Fact]
    public async Task Publish_UserCreatedEvent_Should_Log_Info()
    {
        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddConsole());

        services.AddMinimalMediatR(
            typeof(UserCreatedEvent).Assembly,
            typeof(UserCreatedEventHandler).Assembly);

        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();

        var notification = new UserCreatedEvent
        {
            UserId = 99,
            UserName = "Adnan"
        };

        await mediator.Publish(notification, CancellationToken.None);
    }
}