﻿using DynamicDriving.DriverManagement.Core.Drivers;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicDriving.DriverManagement.Core;

public static class CoreExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IDriverService, DriverService>();

        return services;
    }
}
