using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using DynamicDriving.Events;

namespace DynamicDriving.EventBus.Serializers.Contexts;

public class DriverCreatedContextFactory : IEventContextFactory
{
    public Type ContextType => typeof(DriverCreated);

    public JsonSerializerContext GetContext() => DriverCreatedContext.Default;

    public JsonTypeInfo GetJsonTypeInfo() => DriverCreatedContext.Default.DriverCreated;
}

[JsonSerializable(typeof(DriverCreated), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class DriverCreatedContext : JsonSerializerContext
{
}
