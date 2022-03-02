using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.LocationsAggregate.Services;

public interface ILocationProvider
{
    Maybe<Location> GetLocation(Coordinates coordinates);
}
