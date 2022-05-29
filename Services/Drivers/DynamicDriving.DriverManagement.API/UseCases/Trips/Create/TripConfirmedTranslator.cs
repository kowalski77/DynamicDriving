using DynamicDriving.AzureServiceBus.Receiver;
using DynamicDriving.DriverManagement.Core.Trips.Commands;
using DynamicDriving.Events;
using DynamicDriving.SharedKernel;
using MediatR;

namespace DynamicDriving.DriverManagement.API.UseCases.Trips.Create;

public sealed class TripConfirmedTranslator : ITranslator<TripConfirmed>
{
    public INotification Translate(TripConfirmed message)
    {
        Guards.ThrowIfNull(message);

        return new CreateTrip(
            message.Id, 
            message.PickUp,
            message.OriginLatitude, message.OriginLongitude, 
            message.DestinationLatitude, message.DestinationLongitude);
    }
}
