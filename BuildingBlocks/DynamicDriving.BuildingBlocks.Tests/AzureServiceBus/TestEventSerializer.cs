using System;
using System.Text.Json.Serialization;
using DynamicDriving.AzureServiceBus.Serializers;
using DynamicDriving.Events;

namespace DynamicDriving.BuildingBlocks.Tests.AzureServiceBus;

public record TestEvent(Guid Id, string Name, string Value) : IIntegrationEvent;

public class TestEventContextFactory : IEventContextFactory
{
    public Type ContextType => typeof(TestEvent);

    public JsonSerializerContext GetContext() => TestEventContext.Default;
}

[JsonSerializable(typeof(TestEvent), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class TestEventContext : JsonSerializerContext
{
}
