using DynamicDriving.AzureServiceBus.Receiver;
using DynamicDriving.Events;
using DynamicDriving.SharedKernel;
using MediatR;

namespace DynamicDriving.TripManagement.API.UseCases.Trips.Assign;

public class DriverAssignedTranslator : ITranslator<DriverAssigned>
{
    public INotification Translate(DriverAssigned message)
    {
        Guards.ThrowIfNull(message);

        throw new NotImplementedException();
    }
}
