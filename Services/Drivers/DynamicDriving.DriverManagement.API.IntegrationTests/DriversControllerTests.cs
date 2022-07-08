using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using DynamicDriving.Contracts.Events;
using DynamicDriving.Contracts.Models;
using DynamicDriving.SharedKernel.Envelopes;
using FluentAssertions;
using Moq;
using Xunit;

namespace DynamicDriving.DriverManagement.API.IntegrationTests;

[Collection(IntegrationTestConstants.TestWebApplicationFactoryCollection)]
public class DriversControllerTests
{
    private const string DriversEndpoint = "/api/v1/Drivers";

    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    private readonly TestWebApplicationFactory factory;

    public DriversControllerTests(TestWebApplicationFactory factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task Driver_is_registered()
    {
        // Arrange
        var driverId = Guid.NewGuid();
        var request = this.factory.Fixture.Build<RegisterDriverRequest>().With(x => x.Id, driverId).Create();

        // Act
        var responseMessage = await this.factory.TestServer.CreateClient().PostAsJsonAsync(DriversEndpoint, request);

        // Assert
        responseMessage.EnsureSuccessStatusCode();

        var envelope = await responseMessage.Content.ReadFromJsonAsync<SuccessEnvelope<RegisterDriverResponse>>(JsonSerializerOptions);
        envelope!.Data.DriverId.Should().Be(driverId);

        this.factory.PublisherMock.Verify(x => x.Publish(It.Is<DriverCreated>(y => y.DriverId == driverId), typeof(DriverCreated), CancellationToken.None), Times.Once);
    }
}
