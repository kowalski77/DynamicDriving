using DynamicDriving.Contracts.Trips;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Mongo;
using DynamicDriving.TripService.API.Entities;
using MassTransit;

namespace DynamicDriving.TripService.API.Consumers;

public class TripDraftedConsumer : IConsumer<TripDrafted>
{
    private readonly IMongoRepository<Trip> tripRepository;

    public TripDraftedConsumer(IMongoRepository<Trip> tripRepository)
    {
        this.tripRepository = tripRepository;
    }

    public async Task Consume(ConsumeContext<TripDrafted> context)
    {
        Guards.ThrowIfNull(context);
        
        var trip = new Trip(context.Message.TripId, context.Message.Credits);

        await this.tripRepository.CreateAsync(trip, context.CancellationToken).ConfigureAwait(false);
    }
}
