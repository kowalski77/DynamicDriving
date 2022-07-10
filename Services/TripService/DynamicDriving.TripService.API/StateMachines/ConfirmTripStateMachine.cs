using DynamicDriving.Contracts.TripService;
using MassTransit;

namespace DynamicDriving.TripService.API.StateMachines;

public class ConfirmTripStateMachine : MassTransitStateMachine<ConfirmTripState>
{
    private readonly ILogger<ConfirmTripStateMachine> logger;

    public ConfirmTripStateMachine(ILogger<ConfirmTripStateMachine> logger)
    {
        this.logger = logger;

        this.InstanceState(state => state.CurrentState);
        this.ConfigureEvents();
        this.ConfigureInitialState();
    }

    public State? Accepted { get; }

    public State? ItemsGranted { get; }

    public State? Completed { get; }

    public State? Faulted { get; }

    public Event<ConfirmTripRequested>? ConfirmTripRequested { get; }

    private void ConfigureEvents()
    {
        this.Event(() => this.ConfirmTripRequested);
    }

    private void ConfigureInitialState()
    {
        this.Initially(
            When(this.ConfirmTripRequested)
            .Then(context =>
            {
                context.Saga.UserId = context.Message.UserId;
                context.Saga.TripId = context.Message.TripId;
                context.Saga.Credits = context.Message.Credits;
                context.Saga.Received = DateTimeOffset.UtcNow;
                context.Saga.LastUpdated = context.Saga.Received;
            })
            .TransitionTo(this.Accepted));
    }
}
