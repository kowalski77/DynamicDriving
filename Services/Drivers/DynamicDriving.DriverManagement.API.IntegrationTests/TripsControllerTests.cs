using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using DynamicDriving.Models;
using DynamicDriving.SharedKernel.Envelopes;
using FluentAssertions;
using Xunit;

namespace DynamicDriving.DriverManagement.API.IntegrationTests;

[Collection(IntegrationTestConstants.TestWebApplicationFactoryCollection)]
public class TripsControllerTests
{
    private const string TripsEndpoint = "/api/v1/Trips";

    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    private readonly TestWebApplicationFactory factory;

    public TripsControllerTests(TestWebApplicationFactory factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task Driver_is_assigned_successfully_to_an_existing_trip()
    {
        // Arrange
        var request = this.factory.Fixture.Build<AssignDriverRequest>()
            .With(x => x.TripId, Guid.Parse(IntegrationTestConstants.TripId))
            .Create();

        // Act
        var responseMessage = await this.factory.Client.PostAsJsonAsync(TripsEndpoint, request);

        // Assert
        responseMessage.EnsureSuccessStatusCode();
        var response = await responseMessage.Content.ReadFromJsonAsync<SuccessEnvelope<AssignDriverResponse>>(JsonSerializerOptions);
        response!.Data.TripId.Should().Be(IntegrationTestConstants.TripId);
        response.Data.DriverId.Should().NotBeEmpty();
    }
}
