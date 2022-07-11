extern alias Drivers;

using AutoFixture;
using DynamicDriving.DriverManagement.Core.Drivers;
using DynamicDriving.DriverManagement.Core.Trips;
using DynamicDriving.SharedKernel.Mongo;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using DriversProgram = Drivers::Program;

namespace DynamicDriving.SystemTests.Services;

public class DriversWebApplicationFactory : WebApplicationFactory<DriversProgram>
{
    private readonly IFixture fixture = new Fixture();

    public DriversWebApplicationFactory()
    {
        this.TestServer = this.WithWebHostBuilder(builder =>
        {
             builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        "Test", options => { });
            });
        }).Server;
    }

    public TestServer TestServer { get; }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
            {
                config.AddUserSecrets(typeof(DriversWebApplicationFactory).Assembly)
                .AddJsonFile("appsettings.Drivers.json", false)
                .AddEnvironmentVariables("ASPNETCORE");
            })
            .UseEnvironment("Drivers");

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

    private static void DropDatabase(WebHostBuilderContext context)
    {
        var mongoOptions = context.Configuration.GetSection(nameof(MongoOptions)).Get<MongoOptions>();
        var client = new MongoClient(mongoOptions.Client);
        client.DropDatabase(mongoOptions.Database);
    }

    private void SeedDatabase(IServiceScope scope)
    {
        var mongoDatabase = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

        var tripCollection = mongoDatabase.GetCollection<Trip>(nameof(Trip));

        var trip = new Trip(Guid.Parse(SystemTestsConstants.TripId), DateTime.Now, this.fixture.Create<Coordinates>(), this.fixture.Create<Coordinates>());
        tripCollection.InsertOne(trip);

        var driverCollection = mongoDatabase.GetCollection<Driver>(nameof(Driver));
        var driver = new Driver(Guid.Parse(SystemTestsConstants.DriverId), this.fixture.Create<string>(), this.fixture.Create<Car>(), true);
        driverCollection.InsertOne(driver);
    }
}
