extern alias Trips;

using AutoFixture;
using DynamicDriving.SharedKernel.Outbox.Sql;
using DynamicDriving.TripManagement.Domain.CitiesAggregate;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.DriversAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using DynamicDriving.TripManagement.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TripsProgram = Trips::Program;

namespace DynamicDriving.SystemTests.Services;

public class TripsWebApplicationFactory : WebApplicationFactory<TripsProgram>
{
    private IServiceProvider serviceProvider = default!;
    private readonly IFixture fixture = new Fixture();

    public TripsWebApplicationFactory()
    {
        this.HttpClient = this.CreateClient();
    }

    public HttpClient HttpClient { get; }

    public async Task<Driver> GetDriverByIdAsync(Guid id)
    {
        var repository = this.serviceProvider.GetRequiredService<IDriverRepository>();

        return (await repository.GetAsync(id)).Value;
    }

    public async Task<Trip> GetTripByIdAsync(Guid id)
    {
        var repository = this.serviceProvider.GetRequiredService<ITripRepository>();

        return (await repository.GetAsync(id)).Value;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            this.serviceProvider = services.BuildServiceProvider();
            using var scope = this.serviceProvider.CreateScope();

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
        var cityEntry = context.Cities.Add(new City(this.fixture.Create<string>()));
        var originLocation = new Location(Guid.NewGuid(), this.fixture.Create<string>(), cityEntry.Entity, Coordinates.CreateInstance(10, 10).Value);
        var destinationLocation = new Location(Guid.NewGuid(), this.fixture.Create<string>(), cityEntry.Entity, Coordinates.CreateInstance(20, 20).Value);

        context.Trips.Add(new Trip(Guid.Parse(SystemTestsConstants.TripId), UserId.CreateInstance(Guid.NewGuid()).Value, DateTime.Now, originLocation, destinationLocation));

        var driver = new Driver(Guid.Parse(SystemTestsConstants.DriverId), this.fixture.Create<string>(), this.fixture.Create<string>(), this.fixture.Create<Car>());
        context.Drivers.Add(driver);

        context.SaveChanges();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
            builder.ConfigureHostConfiguration(config =>
            {
                config.AddUserSecrets(typeof(TripsWebApplicationFactory).Assembly)
                .AddJsonFile("appsettings.Trips.json", false)
                .AddEnvironmentVariables("ASPNETCORE");
            })
            .UseEnvironment("Trips");

        return base.CreateHost(builder);
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
}
