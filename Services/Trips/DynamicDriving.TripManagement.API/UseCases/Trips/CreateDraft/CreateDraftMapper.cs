using DynamicDriving.Models;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Commands;

namespace DynamicDriving.TripManagement.API.UseCases.Trips.CreateDraft;

public static class CreateDraftMapper
{
    public static CreateDraftTrip AsCommand(this CreateDraftTripModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return new CreateDraftTrip(
            model.UserId, 
            model.PickUp, 
            model.OriginLatitude, 
            model.OriginLatitude, 
            model.DestinationLatitude, 
            model.DestinationLongitude);
    }
}
