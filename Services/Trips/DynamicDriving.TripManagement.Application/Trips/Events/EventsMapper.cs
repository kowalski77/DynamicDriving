using DynamicDriving.Events;
using DynamicDriving.SharedKernel;
using TripConfirmedDomainEvent = DynamicDriving.TripManagement.Domain.TripsAggregate.Events.TripConfirmed;

namespace DynamicDriving.TripManagement.Application.Trips.Events;

public static class EventsMapper
{
    public static TripCreated AsIntegrationEvent(this TripConfirmedDomainEvent source)
    {
        Guards.ThrowIfNull(source);

        return new TripCreated(
            Guid.NewGuid(), 
            source.TripId, 
            source.PickUp, 
            source.Origin.Latitude, 
            source.Origin.Longitude,
            source.Destination.Latitude,
            source.Destination.Longitude);
    }
}
