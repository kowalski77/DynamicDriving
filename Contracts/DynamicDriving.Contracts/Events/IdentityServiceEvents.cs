namespace DynamicDriving.Contracts.Events;

public record CreditsDeducted(Guid CorrelationId);

public record CreditsAdded(Guid CorrelationId);