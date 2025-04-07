using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MinimalMediatR.Core;

namespace MinimalMediatR.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMinimalMediatR(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddSingleton<IMediator, Mediator>();

        var allTypes = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => !t.IsAbstract && !t.IsInterface && t.IsPublic)
            .ToList();

        foreach (var type in allTypes)
        {
            foreach (var handlerInterface in type.GetInterfaces())
            {
                if (!handlerInterface.IsGenericType) continue;

                var definition = handlerInterface.GetGenericTypeDefinition();

                if (definition == typeof(IRequestHandler<,>) ||
                    definition == typeof(INotificationHandler<>) ||
                    definition == typeof(IPipelineBehavior<,>))
                {
                    services.AddTransient(handlerInterface, type);
                }
            }
        }

        return services;
        }
}