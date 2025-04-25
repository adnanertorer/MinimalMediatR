using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MinimalMediatR.Behaviors;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;
using MinimalMediatR.Tests.Handlers;
using MinimalMediatR.Tests.Infrastructure;
using MinimalMediatR.Tests.Models;
using MinimalMediatR.Tests.Queries;

namespace MinimalMediatR.Tests;

public class MediatorTest
{
    private readonly IMediator _mediator;
    
    public MediatorTest()
    {
        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddConsole());
        services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("TestDb"));
        services.AddMinimalMediatR(typeof(GetUserQuery).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>)); // <-- bu satır önemli
        
        var provider = services.BuildServiceProvider();
        _mediator = provider.GetRequiredService<IMediator>();
        
        using var scope = provider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Users.AddRange(
            new User { Id = 1, Name = "Ali Timur Ertorer" },
            new User { Id = 2, Name = "Adnan Ertorer" }
        );
        db.SaveChanges();
    }
    [Fact]
    public async Task GetUserQuery_Should_Return_User()
    {
        var result = await _mediator.Send(new  GetUserQuery()
        {
            Id = 1
        });

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Ali Timur Ertorer", result.Name);
    }
    
    [Fact]
    public async Task GetUsersQuery_Should_Return_List()
    {
        var result = await _mediator.Send(new GetUsersQuery{}, CancellationToken.None);

        Assert.NotNull(result);
    }
}