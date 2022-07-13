namespace DynamicDriving.TripService.API.Settings;

public class QueueSettings
{
    public string? ConfirmTripQueueAddress { get; init; }

    public string? DeductCreditsQueueAddress { get; init; }

    public string? InvalidateTripQueueAddress { get; init; }
}
