extern alias Trips;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TripsProgram = Trips::Program;

namespace DynamicDriving.SystemTests.Services;

public class TripsWebApplicationFactory : WebApplicationFactory<TripsProgram>
{
    public TripsWebApplicationFactory()
    {
        this.HttpClient = this.CreateClient();
    }

    public HttpClient HttpClient { get; }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
            {
                config.AddJsonFile("appsettings.Trips.json", false);
                config.AddEnvironmentVariables("ASPNETCORE");
            })
            .UseEnvironment("Trips");

        return base.CreateHost(builder);
    }
}
