using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using AutoFixture;
using AutoFixture.AutoMoq;
using DynamicDriving.Models;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.LocationsAggregate;
using DynamicDriving.TripManagement.Domain.LocationsAggregate.Services;
using FluentAssertions;
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

    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    private readonly TestWebApplicationFactory factory;
    private readonly IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());

    public TripControllerTests(TestWebApplicationFactory factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task Draft_trip_is_created()
    {
        // Arrange
        var model = new CreateDraftTripModel(Guid.Parse(IntegrationTestConstants.UserId), DateTime.Now, 10, 10, 20, 20);
        var jsonModel = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, JsonMediaType);
        var client = this.factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var locationProviderMock = new Mock<ILocationProvider>();
                locationProviderMock.Setup(x => x.GetLocationAsync(It.IsAny<Coordinates>()))
                    .ReturnsAsync(() => new Location(
                        IntegrationTestConstants.LocationName,
                        IntegrationTestConstants.LocationCityName,
                        Coordinates.CreateInstance(10, 10).Value));

                services.AddScoped(_ => locationProviderMock.Object);
            });
        }).CreateClient();

        // Act
        var response = await client.PostAsync(TripsEndpoint, jsonModel);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = (Envelope)(await response.Content.ReadFromJsonAsync(typeof(Envelope), JsonSerializerOptions))!;
        result.ErrorCode.Should().BeNull();
    }
}
