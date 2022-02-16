using System.Text.Json.Serialization;
using DynamicDriving.Events;

namespace DynamicDriving.AzureServiceBus.Serializers.Contexts;

public class PingContextFactory : IEventContextFactory
{
    public Type ContextType => typeof(Ping);

    public JsonSerializerContext GetContext() => PingContext.Default;
}

[JsonSerializable(typeof(Ping), GenerationMode = JsonSourceGenerationMode.Serialization)]
public partial class PingContext : JsonSerializerContext
{
}
