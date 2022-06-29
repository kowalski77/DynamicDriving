using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Domain.TripsAggregate;

namespace DynamicDriving.TripManagement.Application.Trips.Queries;

public static class TripMappers
{
    public static TripDto AsDto(this Trip trip)
    {
        Guards.ThrowIfNull(trip);

        return new TripDto(trip.UserId.Value, trip.Driver?.Name, trip.PickUp,
            new TripByIdLocationDto(trip.Origin.Name, trip.Origin.City.Name, trip.Origin.Coordinates.Latitude, trip.Origin.Coordinates.Longitude),
            new TripByIdLocationDto(trip.Destination.Name, trip.Destination.City.Name, trip.Destination.Coordinates.Latitude, trip.Destination.Coordinates.Longitude));
    }
}
