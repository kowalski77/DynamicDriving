using DynamicDriving.Events;

namespace DynamicDriving.EventBus.Serializers;

public interface IIntegrationEventSerializer
{
    string Serialize(IIntegrationEvent integrationEvent);

    Task<T> DeserializeAsync<T>(Stream data);
}
