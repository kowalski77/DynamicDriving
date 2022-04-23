using DynamicDriving.AzureServiceBus.Receiver;
using DynamicDriving.DriverManagement.Core.Trips.Commands;
using DynamicDriving.Events;
using DynamicDriving.SharedKernel;
using MediatR;

namespace DynamicDriving.DriverManagement.API.Translators;

public class ExamCreatedTranslator : ITranslator<TripConfirmed>
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
