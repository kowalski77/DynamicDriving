using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using DynamicDriving.Contracts.Trips;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
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
        var model = new CreateDraftTripRequest(DateTime.Now, 10, 10, 20, 20);
        var jsonModel = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, JsonMediaType);

        var client = this.factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var coordinatesAgentMock = new Mock<ICoordinatesAgent>();
                coordinatesAgentMock.Setup(x => x.GetCityByCoordinatesAsync(It.IsAny<Coordinates>(), CancellationToken.None))
                    .ReturnsAsync(IntegrationTestConstants.LocationCityName);
                coordinatesAgentMock.Setup(x => x.GetLocationByCoordinatesAsync(It.IsAny<Coordinates>(), CancellationToken.None))
                    .ReturnsAsync(IntegrationTestConstants.LocationCityName);
                coordinatesAgentMock.Setup(x => x.GetDistanceInKmBetweenCoordinates(It.IsAny<Coordinates>(), It.IsAny<Coordinates>(), CancellationToken.None))
                    .ReturnsAsync(5);

                services.AddScoped(_ => coordinatesAgentMock.Object);

                _ = services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
            });
        }).CreateClient();

        // Act
        var response = await client.PostAsync(TripsEndpoint, jsonModel);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = (SuccessEnvelope<CreateDraftTripResponse>)(await response.Content.ReadFromJsonAsync(typeof(SuccessEnvelope<CreateDraftTripResponse>), JsonSerializerOptions))!;
        result.Data.TripId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Trip_is_retrieved_by_identifier()
    {
        // Arrange
        var dbContext = this.factory.TestServer.Services.GetRequiredService<TripManagementContext>();
        var trip = dbContext.Trips.First();
        
        var client = this.factory.TestServer.CreateClient();
        
        // Act
        var response = await client.GetAsync($"{TripsEndpoint}/{trip.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = (SuccessEnvelope<TripByIdResponse>)(await response.Content.ReadFromJsonAsync(typeof(SuccessEnvelope<TripByIdResponse>), JsonSerializerOptions))!;
        result.Data.Should().NotBeNull();
    }
}
