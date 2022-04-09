using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Domain.TripsAggregate;

namespace DynamicDriving.TripManagement.Application.Trips.Queries;

public static class GetTripByIdMapper
{
    public static TripByIdDto AsDto(this Trip trip)
    {
        Guards.ThrowIfNull(trip);

        return new TripByIdDto(trip.User.Name, trip.Driver?.Name, trip.PickUp,
            new TripByIdLocationDto(trip.Origin.Name, trip.Origin.City, trip.Origin.Coordinates.Latitude, trip.Origin.Coordinates.Longitude),
            new TripByIdLocationDto(trip.Destination.Name, trip.Destination.City, trip.Destination.Coordinates.Latitude, trip.Destination.Coordinates.Longitude));
    }
}
