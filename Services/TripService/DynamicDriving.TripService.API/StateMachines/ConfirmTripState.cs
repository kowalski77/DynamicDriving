using MassTransit;

namespace DynamicDriving.TripService.API.StateMachines;

public class ConfirmTripState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }

    public string CurrentState { get; set; } = default!;

    public Guid UserId { get; set; }

    public Guid TripId { get; set; }

    public int Credits { get; set; }
    
    public DateTimeOffset Received { get; set; }
    
    public DateTimeOffset LastUpdated { get; set; }
    
    public string ErrorMessage { get; set; } = default!;
    
    public int Version { get; set; }
}
