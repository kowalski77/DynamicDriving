using System.Reflection;

namespace DynamicDriving.AzureServiceBus;

public class AzureServiceBusOptions
{
    public string StorageConnectionString { get; set; } = default!;

    public Assembly IntegrationEventsAssembly { get; set; } = default!;
}
