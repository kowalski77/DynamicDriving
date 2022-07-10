namespace DynamicDriving.Contracts.Identity;

public record CreditsDeducted(Guid CorrelationId);

public record CreditsAdded(Guid CorrelationId);