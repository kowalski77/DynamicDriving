using DynamicDriving.DriverManagement.Core.Drivers;
using DynamicDriving.DriverManagement.Core.Outbox;
using DynamicDriving.DriverManagement.Core.Trips.Commands;
using DynamicDriving.SharedKernel.Application;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicDriving.DriverManagement.Core;

public static class CoreExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddServiceCommandsFromAssembly<AssignDriverServiceCommand>();
        services.AddScoped<IDriverService, DriverService>();
        services.AddScoped<IOutboxService, OutboxService>();

        return services;
    }
}
