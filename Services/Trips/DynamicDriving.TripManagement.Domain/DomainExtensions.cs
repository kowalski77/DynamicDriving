﻿using DynamicDriving.TripManagement.Domain.TripsAggregate.Services;
using Microsoft.Extensions.DependencyInjection;

[assembly: CLSCompliant(false)]
namespace DynamicDriving.TripManagement.Domain;

public static class DomainExtensions
{
    public static void AddDomainServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<ITripService, TripService>();
        services.AddScoped<ILocationFactory, LocationFactory>();
        services.AddScoped<ITripValidator, TripValidator>();
        services.AddScoped<ITripCostCalculator, TripCostCalculator>();
    }
}
