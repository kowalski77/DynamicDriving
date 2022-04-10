using DynamicDriving.Models;
using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Commands;

namespace DynamicDriving.TripManagement.API.UseCases.Trips.CreateDraft;

public static class CreateDraftMapper
{
    public static CreateDraftTrip AsCommand(this CreateDraftTripRequest request)
    {
        Guards.ThrowIfNull(request);

        return new CreateDraftTrip(
            request.TripId,
            request.UserId, 
            request.PickUp, 
            request.OriginLatitude, 
            request.OriginLatitude, 
            request.DestinationLatitude, 
            request.DestinationLongitude);
    }

    public static CreateDraftTripResponse AsResponse(this DraftTripDto source)
    {
        Guards.ThrowIfNull(source);

        return new CreateDraftTripResponse(source.Id);
    }
}
