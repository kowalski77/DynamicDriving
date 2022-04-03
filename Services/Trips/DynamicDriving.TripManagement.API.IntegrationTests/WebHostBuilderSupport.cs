using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.LocationsAggregate;
using DynamicDriving.TripManagement.Domain.LocationsAggregate.Services;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace DynamicDriving.TripManagement.API.IntegrationTests;

public static class WebHostBuilderSupport
{
    public static HttpClient CreateClientWithMockLocationProvider(this TestWebApplicationFactory factory, Location location)
    {
        var client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var locationProviderMock = new Mock<ILocationProvider>();
                locationProviderMock.Setup(x => x.GetLocationAsync(It.IsAny<Coordinates>()))
                    .ReturnsAsync(() => location);

                services.AddScoped(_ => locationProviderMock.Object);
            });
        }).CreateClient();

        return client;
    }
}
