using DynamicDriving.EventBus;
using DynamicDriving.Events;
using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Application.Drivers.Commands;
using MediatR;

namespace DynamicDriving.TripManagement.API.UseCases.Drivers.Create;

public class DriverCreatedTranslator : ITranslator<DriverCreated>
{
    public INotification Translate(DriverCreated message)
    {
        Guards.ThrowIfNull(message);

        return new CreateDriver(
            message.DriverId,
            message.Name,
            message.CarName,
            message.CarDescription);
    }
}
