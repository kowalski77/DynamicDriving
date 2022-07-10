using DynamicDriving.Contracts.Drivers;
using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Application.Drivers.Commands;

namespace DynamicDriving.TripManagement.API.UseCases.Drivers.Create;

public static class DriverCreatedMapper
{
    public static CreateDriver AsCommand(this DriverCreated message)
    {
        Guards.ThrowIfNull(message);

        return new CreateDriver(
            message.DriverId,
            message.Name,
            message.CarName,
            message.CarDescription);
    }
}
