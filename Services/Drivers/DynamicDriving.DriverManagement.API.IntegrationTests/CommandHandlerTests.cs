using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using DynamicDriving.AzureServiceBus.Receiver;
using DynamicDriving.Events;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace DynamicDriving.DriverManagement.API.IntegrationTests;

[Collection(IntegrationTestConstants.TestWebApplicationFactoryCollection)]
public class CommandHandlerTests
{
    private readonly TestWebApplicationFactory factory;

    public CommandHandlerTests(TestWebApplicationFactory factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task Trip_is_stored_when_trip_confirmed_event_is_received()
    {
        // Arrange
        var client = this.factory.WithWebHostBuilder(builder =>
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
        });
        _ = client.CreateDefaultClient();

        var message = this.factory.Fixture.Create<TripConfirmed>();
        var consumer = this.factory.GetConsumer<TripConfirmed>();

        // Act
        await consumer.ExecuteAsync(message);

        // Assert

    }
}
