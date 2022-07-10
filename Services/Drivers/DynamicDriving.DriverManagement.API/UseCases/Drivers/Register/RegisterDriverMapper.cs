using DynamicDriving.Contracts.Drivers;
using DynamicDriving.DriverManagement.Core.Drivers.Commands;
using DynamicDriving.SharedKernel;
using CarType = DynamicDriving.DriverManagement.Core.Drivers.CarType;

namespace DynamicDriving.DriverManagement.API.UseCases.Drivers.Register;

public static class RegisterDriverMapper
{
    public static RegisterDriver AsCommand(this RegisterDriverRequest request)
    {
        Guards.ThrowIfNull(request);

        return new RegisterDriver(
            request.Id,
            request.Name,
            new CarModel(request.Car.Model, (CarType)request.Car.CarType),
            request.IsAvailable);
    }
}
