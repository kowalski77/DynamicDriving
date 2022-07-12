using DynamicDriving.Contracts.TripService;
using MassTransit;

namespace DynamicDriving.TripService.API.StateMachines;

public class BookingStateMachine : MassTransitStateMachine<BookingState>
{
    private readonly ILogger<BookingStateMachine> logger;

    public BookingStateMachine(ILogger<BookingStateMachine> logger)
    {
        this.logger = logger;

        this.InstanceState(state => state.CurrentState);
        this.ConfigureEvents();
        this.ConfigureInitialState();
        this.ConfigureAny();
    }

    public State? Accepted { get; }

    public State? ItemsGranted { get; }

    public State? Completed { get; }

    public State? Faulted { get; }

    public Event<BookingRequested>? BookingRequested { get; }

    public Event<GetBookingState>? GetBookingState { get; }

    private void ConfigureEvents()
    {
        this.Event(() => this.BookingRequested);
        this.Event(() => this.GetBookingState);
    }

    private void ConfigureInitialState()
    {
        this.Initially(
            When(this.BookingRequested)
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

    private void ConfigureAny()
    {
        DuringAny(
            When(this.GetBookingState)
            .Respond(x => x.Saga));
    }
}
