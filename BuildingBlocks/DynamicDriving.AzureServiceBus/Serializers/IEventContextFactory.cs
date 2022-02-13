using System.Text.Json.Serialization;

namespace DynamicDriving.AzureServiceBus.Serializers;

public interface IEventContextFactory
{
    Type ContextType { get; }

    JsonSerializerContext GetContext();
}
