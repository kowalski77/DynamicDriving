using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using DynamicDriving.Events;

namespace DynamicDriving.EventBus.Serializers.Contexts;

public class TripConfirmedContextFactory : IEventContextFactory
{
    public Type ContextType => typeof(TripConfirmed);

    public JsonSerializerContext GetContext() => TripConfirmedContext.Default;

    public JsonTypeInfo GetJsonTypeInfo() => TripConfirmedContext.Default.TripConfirmed;
}

[JsonSerializable(typeof(TripConfirmed), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class TripConfirmedContext : JsonSerializerContext
{
}
