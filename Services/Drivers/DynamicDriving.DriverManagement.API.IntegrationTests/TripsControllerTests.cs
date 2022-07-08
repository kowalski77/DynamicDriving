using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using DynamicDriving.Contracts.Events;
using DynamicDriving.Contracts.Models;
using DynamicDriving.DriverManagement.Core.Trips;
using DynamicDriving.SharedKernel.Envelopes;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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
        var tripId = Guid.Parse(IntegrationTestConstants.TripId);
        var request = this.factory.Fixture.Build<AssignDriverRequest>()
            .With(x => x.TripId, tripId)
            .Create();

        // Act
        var responseMessage = await this.factory.TestServer.CreateClient().PostAsJsonAsync(TripsEndpoint, request);

        // Assert
        responseMessage.EnsureSuccessStatusCode();

        var response = await responseMessage.Content.ReadFromJsonAsync<SuccessEnvelope<AssignDriverResponse>>(JsonSerializerOptions);
        response!.Data.TripId.Should().Be(IntegrationTestConstants.TripId);

        var repository = this.factory.TestServer.Services.GetRequiredService<ITripRepository>();
        var trip = await repository.GetAsync(tripId);
        trip!.Driver?.Id.Should().Be(response.Data.DriverId);
        this.factory.PublisherMock.Verify(x => x.Publish(It.Is<DriverAssigned>(y => y.TripId == tripId), typeof(DriverAssigned), CancellationToken.None), Times.Once);
    }
}
