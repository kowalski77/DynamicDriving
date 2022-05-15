using DynamicDriving.DriverManagement.API.IntegrationTests;
using Xunit;

// ReSharper disable once CheckNamespace
namespace DynamicDriving.TripManagement.API.IntegrationTests;

[CollectionDefinition(IntegrationTestConstants.TestWebApplicationFactoryCollection)]
public class TestWebApplicationFactoryCollection : ICollectionFixture<TestWebApplicationFactory>
{
}
