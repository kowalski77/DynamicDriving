using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using DynamicDriving.Events;

namespace DynamicDriving.EventBus.Serializers.Contexts;

public class DriverAssignedContextFactory : IEventContextFactory
{
    public Type ContextType => typeof(DriverAssigned);

    public JsonSerializerContext GetContext() => DriverAssignedContext.Default;

    public JsonTypeInfo GetJsonTypeInfo() => DriverAssignedContext.Default.DriverAssigned;
}

[JsonSerializable(typeof(DriverAssigned), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class DriverAssignedContext : JsonSerializerContext
{
}
