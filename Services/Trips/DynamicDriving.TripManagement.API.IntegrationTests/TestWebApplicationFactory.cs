﻿using AutoFixture;
using DynamicDriving.AzureServiceBus.Receiver;
using DynamicDriving.SharedKernel.Outbox.Sql;
using DynamicDriving.TripManagement.Domain.CitiesAggregate;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.DriversAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using DynamicDriving.TripManagement.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DynamicDriving.TripManagement.API.IntegrationTests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly Lazy<HttpClient> httpClient;
    private IServiceProvider serviceProvider = default!;

    public TestWebApplicationFactory()
    {
        this.httpClient = new Lazy<HttpClient>(() =>
        {
            return this.WithWebHostBuilder(builder =>
            {
                _ = builder.ConfigureTestServices(services =>
                {
                    _ = services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "Test", options => { });
                });
            }).CreateDefaultClient();
        });
    }

    public IFixture Fixture { get; } = new Fixture();

    public HttpClient Client => this.httpClient.Value;

    public IConsumer<T> GetConsumer<T>()
    {
        return this.serviceProvider.GetRequiredService<IConsumer<T>>();
    }

    public async Task<Driver> GetDriverByIdAsync(Guid id)
    {
        var repository = this.serviceProvider.GetRequiredService<IDriverRepository>();
        var driver = await repository.GetAsync(id);

        return driver.Value;
    }

    public async Task<Trip> GetTripByIdAsync(Guid id)
    {
        var repository = this.serviceProvider.GetRequiredService<ITripRepository>();
        var trip = await repository.GetAsync(id);

        return trip.Value;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        _ = builder.ConfigureHostConfiguration(config =>
            {
                _ = config.AddJsonFile("appsettings.Testing.json", false);
                _ = config.AddEnvironmentVariables("ASPNETCORE");
            })
            .UseEnvironment("Testing");

        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _ = builder.ConfigureServices(services =>
        {
            this.serviceProvider = services.BuildServiceProvider();
            using var scope = this.serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TripManagementContext>();
            var outboxContext = scope.ServiceProvider.GetRequiredService<OutboxContext>();

            _ = context.Database.EnsureDeleted();
            context.Database.Migrate();
            outboxContext.Database.Migrate();

            this.SeedDatabase(context);
        });

        base.ConfigureWebHost(builder);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            switch (this.serviceProvider)
            {
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
            }
        }

        base.Dispose(disposing);
    }

    private void SeedDatabase(TripManagementContext context)
    {
        var cityEntry = context.Cities.Add(new City(IntegrationTestConstants.LocationCityName));
        var originLocation = new Location(Guid.NewGuid(), IntegrationTestConstants.LocationName, cityEntry.Entity, Coordinates.CreateInstance(IntegrationTestConstants.Latitude, IntegrationTestConstants.Longitude).Value);
        var destinationLocation = new Location(Guid.NewGuid(), IntegrationTestConstants.LocationName2, cityEntry.Entity, Coordinates.CreateInstance(IntegrationTestConstants.Latitude, IntegrationTestConstants.Longitude).Value);

        _ = context.Trips.Add(new Trip(Guid.Parse(IntegrationTestConstants.TripId), UserId.CreateInstance(Guid.NewGuid()).Value, DateTime.Now, originLocation, destinationLocation));

        var driver = new Driver(Guid.Parse(IntegrationTestConstants.DriverId), this.Fixture.Create<string>(), this.Fixture.Create<string>(), this.Fixture.Create<Car>());
        _ = context.Drivers.Add(driver);

        _ = context.SaveChanges();
    }
}
