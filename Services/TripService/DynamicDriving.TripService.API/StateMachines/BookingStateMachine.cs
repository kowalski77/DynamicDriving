using DynamicDriving.Contracts.Trips;
using DynamicDriving.Contracts.TripService;
using DynamicDriving.TripService.API.Activities;
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
        this.ConfigureAccepted();
    }

    public State? Accepted { get; }

    public State? Confirmed { get; }

    public State? Completed { get; }

    public State? Faulted { get; }

    public Event<BookingRequested>? BookingRequested { get; }

    public Event<GetBookingState>? GetBookingState { get; }

    public Event<TripConfirmed>? TripConfirmed { get; set; }

    private void ConfigureEvents()
    {
        this.Event(() => this.BookingRequested);
        this.Event(() => this.GetBookingState);
        this.Event(() => this.TripConfirmed);
    }

    private void ConfigureInitialState()
    {
        this.Initially(
            this.When(this.BookingRequested)
            .Then(context =>
            {
                context.Saga.UserId = context.Message.UserId;
                context.Saga.TripId = context.Message.TripId;
                context.Saga.Credits = context.Message.Credits;
                context.Saga.Received = DateTimeOffset.UtcNow;
                context.Saga.LastUpdated = context.Saga.Received;
            })
            .Activity(x => x.OfType<CalculateBookingTotalActivity>())
            .Send(context => new ConfirmTrip(context.Saga.TripId, context.Saga.CorrelationId))
            .TransitionTo(this.Accepted)
            .Catch<Exception>(e => e.Then(context =>
            {
                context.Saga.ErrorMessage = context.Exception.Message;
                context.Saga.LastUpdated = DateTimeOffset.UtcNow;
            })
            .TransitionTo(this.Faulted)));
    }

    private void ConfigureAccepted()
    {
        this.During(this.Accepted,
            this.When(this.TripConfirmed)
            .Then(context =>
            {
                context.Saga.LastUpdated = DateTimeOffset.UtcNow;
            })
            .TransitionTo(this.Confirmed));
    }

    private void ConfigureAny()
    {
        this.DuringAny(
            this.When(this.GetBookingState)
            .Respond(x => x.Saga));
    }
}
