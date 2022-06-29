using Xunit;

// ReSharper disable once CheckNamespace
namespace DynamicDriving.DriverManagement.API.IntegrationTests;

[CollectionDefinition(IntegrationTestConstants.TestWebApplicationFactoryCollection)]
public class TestWebApplicationFactoryCollection : ICollectionFixture<TestWebApplicationFactory>
{
}
