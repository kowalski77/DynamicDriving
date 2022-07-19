namespace DynamicDriving.Contracts.Identity;

public record DeductCredits(Guid UserId, int Credits, Guid CorrelationId);
