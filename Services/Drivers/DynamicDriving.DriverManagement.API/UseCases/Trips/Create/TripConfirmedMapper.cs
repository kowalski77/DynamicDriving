using DynamicDriving.DriverManagement.Core.Trips.Commands;
using DynamicDriving.Events;
using DynamicDriving.SharedKernel;

namespace DynamicDriving.DriverManagement.API.UseCases.Trips.Create;

public static class TripConfirmedMapper
{
    public static CreateTrip AsCommand(this TripConfirmed message)
    {
        Guards.ThrowIfNull(message);

        return new CreateTrip(
            message.TripId,
            message.PickUp,
            message.OriginLatitude, message.OriginLongitude,
            message.DestinationLatitude, message.DestinationLongitude);
    }
}
