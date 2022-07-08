using System;
using AutoFixture;
using DynamicDriving.DriverManagement.Core.Drivers;
using DynamicDriving.DriverManagement.Core.Trips;
using DynamicDriving.SharedKernel.Mongo;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Moq;

namespace DynamicDriving.DriverManagement.API.IntegrationTests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    public TestWebApplicationFactory()
    {
        this.TestServer = this.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(_ => this.PublisherMock.Object);
                services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
            });
        }).Server;
    }

    public IFixture Fixture { get; } = new Fixture();

    public TestServer TestServer { get; }

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
        builder.ConfigureServices((context, services) =>
        {
            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            DropDatabase(context);
            this.SeedDatabase(scope);
        });

        base.ConfigureWebHost(builder);
    }

    private void SeedDatabase(IServiceScope serviceScope)
    {
        var mongoDatabase = serviceScope.ServiceProvider.GetRequiredService<IMongoDatabase>();

        var tripCollection = mongoDatabase.GetCollection<Trip>(nameof(Trip));

        var trip = new Trip(Guid.Parse(IntegrationTestConstants.TripId), DateTime.Now, this.Fixture.Create<Coordinates>(), this.Fixture.Create<Coordinates>());
        tripCollection.InsertOne(trip);

        var driverCollection = mongoDatabase.GetCollection<Driver>(nameof(Driver));
        var driver = new Driver(Guid.NewGuid(), this.Fixture.Create<string>(), this.Fixture.Create<Car>(), true);
        driverCollection.InsertOne(driver);
    }

    private static void DropDatabase(WebHostBuilderContext context)
    {
        var mongoOptions = context.Configuration.GetSection(nameof(MongoOptions)).Get<MongoOptions>();
        var client = new MongoClient(mongoOptions.Client);
        client.DropDatabase(mongoOptions.Database);
    }
}
