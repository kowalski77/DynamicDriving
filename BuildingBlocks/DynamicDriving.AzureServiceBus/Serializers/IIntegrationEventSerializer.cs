using DynamicDriving.Events;

namespace DynamicDriving.AzureServiceBus.Serializers;

public interface IIntegrationEventSerializer
{
    string Serialize(IIntegrationEvent integrationEvent);
}
