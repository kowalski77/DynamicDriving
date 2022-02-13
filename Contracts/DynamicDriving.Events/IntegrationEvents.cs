namespace DynamicDriving.Events;

public record TestEvent(Guid Id, string Name, string Value) : IIntegrationEvent;
