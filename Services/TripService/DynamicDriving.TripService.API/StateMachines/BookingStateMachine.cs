using DynamicDriving.Contracts.Identity;
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
        this.ConfigureTripConfirmed();
        this.ConfigureFaulted();
        this.ConfigureCompleted();
    }

    public State? Accepted { get; }

    public State? Confirmed { get; }

    public State? Completed { get; }

    public State? Faulted { get; }

    public Event<BookingRequested>? BookingRequested { get; }

    public Event<GetBookingState>? GetBookingState { get; }

    public Event<TripConfirmed>? TripConfirmed { get; }

    public Event<CreditsDeducted>? CreditsDeducted { get; }

    public Event<Fault<ConfirmTrip>>? ConfirmTripFaulted { get; }

    public Event<Fault<DeductCredits>>? DeductCreditsFaulted { get; }


    private void ConfigureEvents()
    {
        this.Event(() => this.BookingRequested);
        this.Event(() => this.GetBookingState);
        this.Event(() => this.TripConfirmed);
        this.Event(() => this.CreditsDeducted);
        this.Event(() => this.ConfirmTripFaulted, x => x.CorrelateById(context => context.Message.Message.CorrelationId));
        this.Event(() => this.DeductCreditsFaulted, x => x.CorrelateById(context => context.Message.Message.CorrelationId));
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

                this.logger.LogInformation("Calculating total booking with correlation id: {CorrelationId}", context.Saga.CorrelationId);
            })
            .Activity(x => x.OfType<CalculateBookingTotalActivity>())
            .Send(context => new ConfirmTrip(
                context.Saga.TripId,
                context.Saga.CorrelationId))
            .TransitionTo(this.Accepted)
            .Catch<Exception>(e => e.Then(context =>
            {
                context.Saga.ErrorMessage = context.Exception.Message;
                context.Saga.LastUpdated = DateTimeOffset.UtcNow;

                this.logger.LogError(context.Exception, "Could not calculate the total booking with correlation id: {CorrelationId}, Error: {Error}", context.Saga.CorrelationId, context.Saga.ErrorMessage);
            })
            .TransitionTo(this.Faulted)));
    }

    private void ConfigureAccepted()
    {
        this.During(this.Accepted,
            this.Ignore(this.BookingRequested),
            this.When(this.TripConfirmed)
            .Then(context =>
            {
                context.Saga.LastUpdated = DateTimeOffset.UtcNow;
                this.logger.LogInformation("Trip accepted for user with id: {UserId} with correlation id: {CorrelationId}", context.Saga.UserId, context.Saga.CorrelationId);
            })
            .Send(context => new DeductCredits(
                context.Saga.UserId,
                context.Saga.Credits,
                context.Saga.CorrelationId))
            .TransitionTo(this.Confirmed),
            this.When(this.ConfirmTripFaulted)
            .Then(context =>
            {
                context.Saga.ErrorMessage = context.Message.Exceptions[0].Message;
                context.Saga.LastUpdated = DateTimeOffset.UtcNow;

                this.logger.LogError("Could not accepted trip for user with id: {UserId} with correlation id: {CorrelationId}, Error: {Error}", context.Saga.UserId, context.Saga.CorrelationId, context.Saga.ErrorMessage);
            })
            .TransitionTo(this.Faulted));
    }

    private void ConfigureTripConfirmed()
    {
        this.During(this.Confirmed,
            this.Ignore(this.BookingRequested),
            this.Ignore(this.TripConfirmed),
            this.When(this.CreditsDeducted)
            .Then(context => 
            { 
                context.Saga.LastUpdated = DateTimeOffset.UtcNow;
                this.logger.LogInformation("Trip confirmed for user with id: {UserId} with correlation id: {CorrelationId}", context.Saga.UserId, context.Saga.CorrelationId);
            })
            .TransitionTo(this.Completed),
            this.When(this.DeductCreditsFaulted)
            .Send(context => new InvalidateTrip(context.Saga.TripId, context.Saga.CorrelationId))
            .Then(context =>
            {
                context.Saga.ErrorMessage = context.Message.Exceptions[0].Message;
                context.Saga.LastUpdated = DateTimeOffset.UtcNow;

                this.logger.LogError("Could not confirm trip for user with id: {UserId} with correlation id: {CorrelationId}, Error: {Error}", context.Saga.UserId, context.Saga.CorrelationId, context.Saga.ErrorMessage);
            })
            .TransitionTo(this.Faulted));
    }

    private void ConfigureCompleted()
    {
        this.During(this.Completed,
            this.Ignore(this.BookingRequested),
            this.Ignore(this.TripConfirmed),
            this.Ignore(this.CreditsDeducted));
    }

    private void ConfigureAny()
    {
        this.DuringAny(
            this.When(this.GetBookingState)
            .Respond(x => x.Saga));
    }

    private void ConfigureFaulted()
    {
        this.During(this.Faulted,
            this.Ignore(this.BookingRequested),
            this.Ignore(this.TripConfirmed),
            this.Ignore(this.CreditsDeducted));
    }
}
