using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using DynamicDriving.Models;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.CitiesAggregate;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
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

    public TripControllerTests(TestWebApplicationFactory factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task Draft_trip_is_created()
    {
        // Arrange
        var tripId = Guid.NewGuid();
        var model = new CreateDraftTripRequest(tripId, Guid.Parse(IntegrationTestConstants.UserId), DateTime.Now, 10, 10, 20, 20);
        var jsonModel = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, JsonMediaType);
        var city = new City(IntegrationTestConstants.LocationCityName);
        var location = new Location(Guid.NewGuid(), IntegrationTestConstants.LocationName, city, Coordinates.CreateInstance(10, 10).Value);

        var client = this.factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var coordinatesAgentMock = new Mock<ICoordinatesAgent>();
                coordinatesAgentMock.Setup(x => x.GetCityByCoordinatesAsync(It.IsAny<Coordinates>(), CancellationToken.None))
                    .ReturnsAsync(Result.Ok(city));
                coordinatesAgentMock.Setup(x => x.GetLocationByCoordinatesAsync(It.IsAny<Coordinates>(), CancellationToken.None))
                    .ReturnsAsync(Result.Ok(location));

                services.AddScoped(_ => coordinatesAgentMock.Object);
            });
        }).CreateClient();

        // Act
        var response = await client.PostAsync(TripsEndpoint, jsonModel);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = (SuccessEnvelope<CreateDraftTripResponse>)(await response.Content.ReadFromJsonAsync(typeof(SuccessEnvelope<CreateDraftTripResponse>), JsonSerializerOptions))!;
        result.Data.TripId.Should().Be(tripId);
    }

    [Fact]
    public async Task Trip_is_retrieved_by_identifier()
    {
        // Arrange
        var client = this.factory.CreateDefaultClient();

        // Act
        var response = await client.GetAsync($"{TripsEndpoint}/{IntegrationTestConstants.TripId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = (SuccessEnvelope<TripByIdResponse>)(await response.Content.ReadFromJsonAsync(typeof(SuccessEnvelope<TripByIdResponse>), JsonSerializerOptions))!;
        result.Data.Should().NotBeNull();
    }
}
