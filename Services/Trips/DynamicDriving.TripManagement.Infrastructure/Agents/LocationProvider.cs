using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.LocationsAggregate;
using DynamicDriving.TripManagement.Domain.LocationsAggregate.Services;

namespace DynamicDriving.TripManagement.Infrastructure.Agents;

public sealed class LocationProvider : ILocationProvider
{
    public Task<Maybe<Location>> GetLocationAsync(Coordinates coordinates)
    {
        throw new NotImplementedException();
    }
}
