using System.Text.Json.Serialization;
using DynamicDriving.Events;

namespace DynamicDriving.AzureServiceBus.Serializers;

public class TestEventContextFactory : IEventContextFactory
{
    public Type ContextType => typeof(TestEvent);

    public JsonSerializerContext GetContext() => TestEventContext.Default;
}

[JsonSerializable(typeof(TestEvent))]
public partial class TestEventContext : JsonSerializerContext
{
}
