extern alias Drivers;
using DynamicDriving.SharedKernel.Mongo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using DriversProgram = Drivers::Program;

namespace DynamicDriving.SystemTests.Services;

public class DriversWebApplicationFactory : WebApplicationFactory<DriversProgram>
{
    private IServiceProvider serviceProvider = default!;

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

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            this.serviceProvider = services.BuildServiceProvider();

            DropDatabase(context);
        });

        base.ConfigureWebHost(builder);
    }

    private static void DropDatabase(WebHostBuilderContext context)
    {
        var mongoOptions = context.Configuration.GetSection(nameof(MongoOptions)).Get<MongoOptions>();
        var client = new MongoClient(mongoOptions.Client);
        client.DropDatabase(mongoOptions.Database);
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
