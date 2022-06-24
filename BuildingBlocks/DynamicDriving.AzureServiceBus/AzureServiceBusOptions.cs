using DynamicDriving.Events;

namespace DynamicDriving.AzureServiceBus;

public class AzureServiceBusOptions
{
    public string StorageConnectionString { get; set; } = default!;

    public ICollection<Type> IntegrationEventTypes { get; } = new List<Type>();

    public void RegisterIntegrationEvent<T>() where T : class, IIntegrationEvent
    {
        this.IntegrationEventTypes.Add(typeof(T));
    }
}
