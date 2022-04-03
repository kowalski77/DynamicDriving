using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using DynamicDriving.Models;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.LocationsAggregate;
using DynamicDriving.TripManagement.Domain.LocationsAggregate.Services;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace DynamicDriving.TripManagement.API.IntegrationTests;

[Collection(IntegrationTestConstants.TestWebApplicationFactoryCollection)]
public class TripControllerTests
{
    private const string JsonMediaType = "application/json";
    private const string TripsEndpoint = "/api/v1/Trips";

    private readonly IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
    private readonly TestWebApplicationFactory factory;

    public TripControllerTests(TestWebApplicationFactory factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task Draft_trip_is_created()
    {
        // Arrange
        var model = new CreateDraftTripModel(Guid.NewGuid(), DateTime.Now, 10, 10, 20, 20);
        var jsonModel = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, JsonMediaType);
        var client = this.factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var locationProviderMock = new Mock<ILocationProvider>();
                locationProviderMock.Setup(x => x.GetLocationAsync(It.IsAny<Coordinates>()))
                    .ReturnsAsync(() => new Location("aa", "aa", Coordinates.CreateInstance(10, 10).Value));

                services.AddScoped(_ => locationProviderMock.Object);

            });
        }).CreateClient();

        // Act
        var response = await client.PostAsync(TripsEndpoint, jsonModel);

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
