using DynamicDriving.Contracts.Events;
using DynamicDriving.DriverManagement.Core.Trips.Commands;
using DynamicDriving.SharedKernel;

namespace DynamicDriving.DriverManagement.API.UseCases.Trips.Create;

public static class TripCreatedMapper
{
    public static CreateTrip AsCommand(this TripCreated message)
    {
        Guards.ThrowIfNull(message);

        return new CreateTrip(
            message.TripId,
            message.PickUp,
            message.OriginLatitude, message.OriginLongitude,
            message.DestinationLatitude, message.DestinationLongitude);
    }
}
