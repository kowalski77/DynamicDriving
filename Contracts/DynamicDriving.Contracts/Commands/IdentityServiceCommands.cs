namespace DynamicDriving.Contracts.Commands;

public record DeductCredits(Guid UserId, int Credits, Guid CorrelationId);

public record AddCredits(Guid UserId, int Credits, Guid CorrelationId);
