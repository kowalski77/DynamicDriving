﻿using DynamicDriving.DriverManagement.Core.Drivers;
using DynamicDriving.DriverManagement.Core.Outbox;
using DynamicDriving.DriverManagement.Core.Trips;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Mongo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicDriving.DriverManagement.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        Guards.ThrowIfNull(configuration);

        var mongoOptions = configuration.GetSection(nameof(MongoOptions)).Get<MongoOptions>();
        services.AddMongo(mongoOptions);
        services.AddScoped<ITripRepository, TripRepository>();
        services.AddScoped<IDriverRepository, DriverRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();

        return services;
    }
}
