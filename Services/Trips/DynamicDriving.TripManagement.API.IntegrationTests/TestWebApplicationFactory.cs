using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.LocationsAggregate;
using DynamicDriving.TripManagement.Domain.UsersAggregate;
using DynamicDriving.TripManagement.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DynamicDriving.TripManagement.API.IntegrationTests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private IServiceProvider serviceProvider = default!;

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
            this.serviceProvider = services.BuildServiceProvider();
            using var scope = this.serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TripManagementContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            SeedDatabase(context);
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

    private static void SeedDatabase(TripManagementContext context)
    {
        context.Users.Add(new User(Guid.Parse(IntegrationTestConstants.UserId), IntegrationTestConstants.UserName));
        context.Locations.Add(new Location(
            IntegrationTestConstants.LocationName,
            IntegrationTestConstants.LocationCityName,
            Coordinates.CreateInstance(IntegrationTestConstants.Latitude, IntegrationTestConstants.Longitude).Value));

        context.SaveChanges();
    }
}
