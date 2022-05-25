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
public class DriverControllerTests
{
    private const string DriversEndpoint = "/api/v1/Drivers";

    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    private readonly TestWebApplicationFactory factory;

    public DriverControllerTests(TestWebApplicationFactory factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task Driver_is_created()
    {
        // Arrange
        var driverId = Guid.NewGuid();
        var request = this.factory.Fixture.Build<RegisterDriverRequest>().With(x => x.Id, driverId).Create();

        // Act
        var responseMessage = await this.factory.Client.PostAsJsonAsync(DriversEndpoint, request);

        // Assert
        var response = await responseMessage.Content.ReadFromJsonAsync<SuccessEnvelope<RegisterDriverResponse>>(JsonSerializerOptions);
        response!.Data.DriverId.Should().Be(driverId);
    }
}
