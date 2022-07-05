using Microsoft.Extensions.DependencyInjection;

namespace DynamicDriving.EventBus;

public static class EventBusExtensions
{
    public static IServiceCollection AddTranslator<TEvent, TTranslator>(this IServiceCollection services)
        where TTranslator : class, ITranslator<TEvent>
    {
        services.AddScoped<ITranslator<TEvent>, TTranslator>();

        return services;
    }
}
