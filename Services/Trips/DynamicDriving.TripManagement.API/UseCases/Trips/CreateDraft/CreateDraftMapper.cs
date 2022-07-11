using DynamicDriving.Contracts.Trips;
using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Application.Trips.Commands;
using DynamicDriving.TripManagement.Domain.TripsAggregate;

namespace DynamicDriving.TripManagement.API.UseCases.Trips.CreateDraft;

public static class CreateDraftMapper
{
    public static CreateDraftTrip AsCommand(this CreateDraftTripRequest request, Guid userId)
    {
        Guards.ThrowIfNull(request);

        return new CreateDraftTrip(
            userId, 
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
