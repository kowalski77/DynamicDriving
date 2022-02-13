using System;
using System.Text.Json;
using AutoFixture;
using DynamicDriving.AzureServiceBus.Serializers;
using DynamicDriving.Events;
using FluentAssertions;
using Xunit;

namespace DynamicDriving.BuildingBlocks.Tests.AzureServiceBus;

public class IntegrationEventSerializerTests
{
    private readonly IFixture fixture = new Fixture();

    [Fact]
    public void Serialize_when_type_with_JsonSerializerContext_is_provided_then_the_object_is_serialized_accordingly()
    {
        // Arrange
        this.fixture.Register<IIntegrationEventSerializer>(()=> 
            new IntegrationEventSerializer(new IEventContextFactory[]{new TestEventContextFactory()}));
        var eventSerializer = this.fixture.Create<IIntegrationEventSerializer>();
        var testEvent = this.fixture.Create<TestEvent>();

        //Act
        var serializedEvent = eventSerializer.Serialize(testEvent);

        //Assert
        var result = JsonSerializer.Deserialize<TestEvent>(serializedEvent);
        result.Should().Be(testEvent);
    }

    [Fact]
    public void Serialize_when_type_without_JsonSerializerContext_is_provided_then_the_operation_is_invalid()
    {
        // Arrange
        this.fixture.Register<IIntegrationEventSerializer>(()=> 
            new IntegrationEventSerializer(Array.Empty<IEventContextFactory>()));
        var eventSerializer = this.fixture.Create<IIntegrationEventSerializer>();
        var testEvent = this.fixture.Create<TestEvent>();

        // Act
        var serializeAction = () => eventSerializer.Serialize(testEvent);

        // Assert
        serializeAction.Should().Throw<InvalidOperationException>();
    }
}
