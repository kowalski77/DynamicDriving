using AutoFixture;
using DynamicDriving.SharedKernel.Outbox.Sql;
using DynamicDriving.TripManagement.Domain.CitiesAggregate;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.DriversAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using DynamicDriving.TripManagement.Infrastructure.Persistence;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;

namespace DynamicDriving.TripManagement.API.IntegrationTests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly Lazy<HttpClient> httpClient;

    public TestWebApplicationFactory()
    {
        this.httpClient = new Lazy<HttpClient>(() =>
        {
            return this.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton(_ => this.PublisherMock.Object);
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "Test", options => { });
                });
            }).CreateDefaultClient();
        });
    }

    public IFixture Fixture { get; } = new Fixture();

    public HttpClient Client => this.httpClient.Value;

    public Mock<IPublishEndpoint> PublisherMock { get; } = new();

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
            {
                config.AddJsonFile("appsettings.Testing.json", false);
                config.AddEnvironmentVariables("ASPNETCORE");
            })
            .UseEnvironment("Testing");

        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TripManagementContext>();
            var outboxContext = scope.ServiceProvider.GetRequiredService<OutboxContext>();

            context.Database.EnsureDeleted();
            context.Database.Migrate();
            outboxContext.Database.Migrate();

            this.SeedDatabase(context);
        });

        base.ConfigureWebHost(builder);
    }

    private void SeedDatabase(TripManagementContext context)
    {
        var cityEntry = context.Cities.Add(new City(IntegrationTestConstants.LocationCityName));
        var originLocation = new Location(Guid.NewGuid(), IntegrationTestConstants.LocationName, cityEntry.Entity, Coordinates.CreateInstance(IntegrationTestConstants.Latitude, IntegrationTestConstants.Longitude).Value);
        var destinationLocation = new Location(Guid.NewGuid(), IntegrationTestConstants.LocationName2, cityEntry.Entity, Coordinates.CreateInstance(IntegrationTestConstants.Latitude, IntegrationTestConstants.Longitude).Value);

        var userId = UserId.CreateInstance(Guid.Parse(IntegrationTestConstants.UserId));
        context.Trips.Add(new Trip(Guid.Parse(IntegrationTestConstants.TripId), userId.Value, DateTime.Now, originLocation, destinationLocation));

        var driver = new Driver(Guid.Parse(IntegrationTestConstants.DriverId), this.Fixture.Create<string>(), this.Fixture.Create<string>(), this.Fixture.Create<Car>());
        context.Drivers.Add(driver);

        context.SaveChanges();
    }
}
