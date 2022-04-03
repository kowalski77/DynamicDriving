using Xunit;

namespace DynamicDriving.TripManagement.API.IntegrationTests;

[CollectionDefinition(IntegrationTestConstants.TestWebApplicationFactoryCollection)]
public class TestWebApplicationFactoryCollection : ICollectionFixture<TestWebApplicationFactory>
{
}
