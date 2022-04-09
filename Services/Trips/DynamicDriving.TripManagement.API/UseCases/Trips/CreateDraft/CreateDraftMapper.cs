using DynamicDriving.Models;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Commands;

namespace DynamicDriving.TripManagement.API.UseCases.Trips.CreateDraft;

public static class CreateDraftMapper
{
    public static CreateDraftTrip AsCommand(this CreateDraftTripRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new CreateDraftTrip(
            request.TripId,
            request.UserId, 
            request.PickUp, 
            request.OriginLatitude, 
            request.OriginLatitude, 
            request.DestinationLatitude, 
            request.DestinationLongitude);
    }
}
