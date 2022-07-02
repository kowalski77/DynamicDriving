namespace DynamicDriving.DriverManagement.Core.Drivers;

public sealed record DriverSummaryDto(Guid Id, string Name, string Car, bool IsAvailable);
