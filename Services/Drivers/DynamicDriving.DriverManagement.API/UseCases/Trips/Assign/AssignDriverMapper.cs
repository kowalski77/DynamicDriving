using DynamicDriving.DriverManagement.Core.Trips.Commands;
using DynamicDriving.Models;
using DynamicDriving.SharedKernel;

namespace DynamicDriving.DriverManagement.API.UseCases.Trips.Assign;

public static class AssignDriverMapper
{
    public static AssignDriver AsCommand(this AssignDriverRequest request)
    {
        Guards.ThrowIfNull(request);

        return new AssignDriver(request.TripId);
    }
}
