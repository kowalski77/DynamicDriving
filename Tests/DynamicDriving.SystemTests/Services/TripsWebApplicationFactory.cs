extern alias Trips;
using DynamicDriving.SharedKernel.Outbox.Sql;
using DynamicDriving.TripManagement.Domain.DriversAggregate;
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
        });

        base.ConfigureWebHost(builder);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
            {
                config.AddUserSecrets(typeof(TripsWebApplicationFactory).Assembly);
                config.AddJsonFile("appsettings.Trips.json", false);
                config.AddEnvironmentVariables("ASPNETCORE");
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
