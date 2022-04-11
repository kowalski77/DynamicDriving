using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.LocationsAggregate;
using DynamicDriving.TripManagement.Domain.LocationsAggregate.Services;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace DynamicDriving.TripManagement.API.IntegrationTests;

public static class WebHostBuilderSupport
{
    public static HttpClient CreateClientWithMockCityProvider(this TestWebApplicationFactory factory, City city)
    {
        var client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var locationProviderMock = new Mock<ICityProvider>();
                locationProviderMock.Setup(x => x.GetCityByCoordinatesAsync(It.IsAny<Coordinates>()))
                    .ReturnsAsync(() => city);

                services.AddScoped(_ => locationProviderMock.Object);
            });
        }).CreateClient();

        return client;
    }
}
