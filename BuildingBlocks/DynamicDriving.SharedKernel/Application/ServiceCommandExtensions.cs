using Microsoft.Extensions.DependencyInjection;

namespace DynamicDriving.SharedKernel.Application;

public static class ServiceCommandExtensions
{
    public static IServiceCollection AddServiceCommandsFromAssembly<T>(this IServiceCollection services)
    {
        var concretes = typeof(T).Assembly.GetTypes()
                .Where(p => p.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IServiceCommand<,>)));

        foreach (var concrete in concretes)
        {
            var interfaceType = concrete.GetInterfaces().First();
            services.AddScoped(interfaceType, concrete);
        }

        return services;
    }
}
