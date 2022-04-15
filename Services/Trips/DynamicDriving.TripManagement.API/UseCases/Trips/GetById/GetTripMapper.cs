using DynamicDriving.Models;
using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Domain.TripsAggregate;

namespace DynamicDriving.TripManagement.API.UseCases.Trips.GetById;

public static class GetTripMapper
{
    public static TripByIdResponse AsResponse(this TripByIdDto source)
    {
        Guards.ThrowIfNull(source);

        return new TripByIdResponse(
            source.UserId,
            source.DriverName,
            source.PickUpTime,
            new TripByIdLocationResponse(source.Origin.Name, source.Origin.City, source.Origin.Latitude, source.Origin.Longitude),
            new TripByIdLocationResponse(source.Destination.Name, source.Destination.City, source.Destination.Latitude, source.Destination.Longitude));
    }
}
