using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.LocationsAggregate.Services;

public interface ILocationProvider
{
    Task<Maybe<Location>> GetLocationAsync(Coordinates coordinates);
}
