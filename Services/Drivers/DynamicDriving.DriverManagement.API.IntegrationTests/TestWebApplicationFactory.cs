using System;
using AutoFixture;
using DynamicDriving.AzureServiceBus.Receiver;
using DynamicDriving.SharedKernel.Mongo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace DynamicDriving.DriverManagement.API.IntegrationTests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private IServiceProvider serviceProvider = default!;

    public IFixture Fixture { get; } = new Fixture();

    public IConsumer<T> GetConsumer<T>()
    {
        using var scope = this.serviceProvider.CreateScope();
        var consumer = this.serviceProvider.GetRequiredService<IConsumer<T>>();

        return consumer;
    }

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
            this.serviceProvider = services.BuildServiceProvider();

            var mongoOptions = context.Configuration.GetSection(nameof(MongoOptions)).Get<MongoOptions>();
            var client = new MongoClient(mongoOptions.Client);
            client.DropDatabase(mongoOptions.Database);
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
}
