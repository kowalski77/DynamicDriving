using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using DynamicDriving.Events;

namespace DynamicDriving.AzureServiceBus.Serializers.Contexts;

public class PingContextFactory : IEventContextFactory
{
    public Type ContextType => typeof(Ping);

    public JsonSerializerContext GetContext() => PingContext.Default;

    public JsonTypeInfo GetJsonTypeInfo() => PingContext.Default.Ping;
}

[JsonSerializable(typeof(Ping), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class PingContext : JsonSerializerContext
{
}
