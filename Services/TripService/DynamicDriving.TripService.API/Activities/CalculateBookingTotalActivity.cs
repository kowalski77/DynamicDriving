using DynamicDriving.Contracts.TripService;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Mongo;
using DynamicDriving.TripService.API.Entities;
using DynamicDriving.TripService.API.Exceptions;
using DynamicDriving.TripService.API.StateMachines;
using MassTransit;

namespace DynamicDriving.TripService.API.Activities;

public class CalculateBookingTotalActivity : IStateMachineActivity<BookingState, BookingRequested>
{
    private readonly IMongoRepository<Trip> tripRepository;

    public CalculateBookingTotalActivity(IMongoRepository<Trip> tripRepository)
    {
        this.tripRepository = tripRepository;
    }

    public void Accept(StateMachineVisitor visitor)
    {
        Guards.ThrowIfNull(visitor);
        
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<BookingState, BookingRequested> context, IBehavior<BookingState, BookingRequested> next)
    {
        Guards.ThrowIfNull(context);
        Guards.ThrowIfNull(next);
        
        var message = context.Message;

        var trip = await tripRepository.GetAsync(message.TripId).ConfigureAwait(false);
        if(trip is null)
        {
            throw new TripNotFoundException(message.TripId);
        }

        // ***********NOTE: this is a ver simplified example *************
        context.Saga.Credits = trip.Credits - 1;
        context.Saga.LastUpdated = DateTimeOffset.UtcNow;

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<BookingState, BookingRequested, TException> context, IBehavior<BookingState, BookingRequested> next) where TException : Exception
    {
        Guards.ThrowIfNull(next);
        
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        Guards.ThrowIfNull(context);
        context.CreateScope("calculate-booking-total");
    }
}
