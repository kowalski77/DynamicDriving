using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using DynamicDriving.Models;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.LocationsAggregate;
using FluentAssertions;
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
        var model = new CreateDraftTripRequest(Guid.NewGuid(), Guid.Parse(IntegrationTestConstants.UserId), DateTime.Now, 10, 10, 20, 20);
        var jsonModel = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, JsonMediaType);
        var location = new Location(
            IntegrationTestConstants.LocationName,
            IntegrationTestConstants.LocationCityName,
            Coordinates.CreateInstance(10, 10).Value);

        var client = this.factory.CreateClientWithMockLocationProvider(location);

        // Act
        var response = await client.PostAsync(TripsEndpoint, jsonModel);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = (Envelope)(await response.Content.ReadFromJsonAsync(typeof(Envelope), JsonSerializerOptions))!;
        result.ErrorCode.Should().BeNull();
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
        var result = (Envelope<TripByIdResponse>)(await response.Content.ReadFromJsonAsync(typeof(Envelope<TripByIdResponse>), JsonSerializerOptions))!;
        result.Result.Should().NotBeNull();
    }
}
