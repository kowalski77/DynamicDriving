using System;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using DynamicDriving.EventBus.Serializers;
using DynamicDriving.Events;

namespace DynamicDriving.BuildingBlocks.Tests.AzureServiceBus;

public record TestEvent(Guid Id, string Name, string Value) : IIntegrationEvent;

public class TestEventContextFactory : IEventContextFactory
{
    public Type ContextType => typeof(TestEvent);

    public JsonSerializerContext GetContext() => TestEventContext.Default;

    public JsonTypeInfo GetJsonTypeInfo() => TestEventContext.Default.TestEvent;
}

[JsonSerializable(typeof(TestEvent), GenerationMode = JsonSourceGenerationMode.Serialization)]
public partial class TestEventContext : JsonSerializerContext
{
}
