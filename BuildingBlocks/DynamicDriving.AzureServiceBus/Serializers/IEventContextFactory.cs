using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace DynamicDriving.AzureServiceBus.Serializers;

public interface IEventContextFactory
{
    Type ContextType { get; }

    JsonSerializerContext GetContext();

    JsonTypeInfo GetJsonTypeInfo();
}
