using System.Reflection;

namespace DynamicDriving.AzureServiceBus;

public class AzureServiceBusOptions
{
    public string StorageConnectionString { get; set; } = default!;

    public IEnumerable<Type> IntegrationEventTypes { get; set; } = new List<Type>();
}
