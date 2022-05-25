﻿using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using DynamicDriving.AzureServiceBus.Receiver;
using DynamicDriving.DriverManagement.Core.Drivers;
using DynamicDriving.DriverManagement.Core.Trips;
using DynamicDriving.SharedKernel.Mongo;
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
    private readonly Lazy<HttpClient> httpClient;
    private IServiceProvider serviceProvider = default!;

    public TestWebApplicationFactory()
    {
        this.httpClient = new Lazy<HttpClient>(() =>
        {
            return this.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var serviceBusClientFactory = new Mock<IServiceBusClientFactory>();
                    services.AddScoped(_ => serviceBusClientFactory.Object);
                    var messageReceiver = new Mock<IMessageReceiver>();
                    services.AddSingleton(_ => messageReceiver.Object);

                    var hostedServiceDescriptor = services.Single(x => x.ImplementationType == typeof(ServiceBusReceiverHostedService));
                    services.Remove(hostedServiceDescriptor);
                });
            }).CreateDefaultClient();
        });
    }

    public IFixture Fixture { get; } = new Fixture();

    public HttpClient Client => this.httpClient.Value;

    public IConsumer<T> GetConsumer<T>()
    {
        return this.serviceProvider.GetRequiredService<IConsumer<T>>();
    }

    public async Task<Trip?> GetTripByIdAsync(Guid id)
    {
        var repository = this.serviceProvider.GetRequiredService<ITripRepository>();

        return await repository.GetAsync(id);
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

            DropDatabase(context);
            this.SeedDatabase();
        });

        base.ConfigureWebHost(builder);
    }

    private void SeedDatabase()
    {
        var mongoDatabase = this.serviceProvider.GetRequiredService<IMongoDatabase>();

        var tripCollection = mongoDatabase.GetCollection<Trip>(nameof(Trip));
        tripCollection.InsertOne(this.Fixture.Create<Trip>());

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