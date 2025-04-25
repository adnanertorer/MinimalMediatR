// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using MinimalMediatR.Behaviors;
using MinimalMediatR.Core;
using MinimalMediatR.ExampleApp.Features.Users;
using MinimalMediatR.Extensions;

var services = new ServiceCollection();
services.AddMinimalMediatR(typeof(CreateUserCommand).Assembly);

services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

var provider = services.BuildServiceProvider();

var mediatr = provider.GetRequiredService<IMediator>();

var createUserResult = await mediatr.Send(new CreateUserCommand
{
    Username = "adnanertorer",
    Email = "adnan@example.com"
});

Console.WriteLine($"[Program] Created User Id: {createUserResult.Id}");

await mediatr.Publish(new UserCreatedNotification
{
    UserId = createUserResult.Id
});

Console.ReadLine();