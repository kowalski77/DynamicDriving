extern alias Drivers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using DriversProgram = Drivers::Program;

namespace DynamicDriving.SystemTests.Services;

public class DriversWebApplicationFactory : WebApplicationFactory<DriversProgram>
{
    public DriversWebApplicationFactory()
    {
        this.HttpClient = this.CreateClient();
    }

    public HttpClient HttpClient { get; }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
            {
                config.AddUserSecrets(typeof(DriversWebApplicationFactory).Assembly);
                config.AddJsonFile("appsettings.Drivers.json", false);
                config.AddEnvironmentVariables("ASPNETCORE");
            })
            .UseEnvironment("Drivers");

        return base.CreateHost(builder);
    }
}
