using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Infrastructure.Agents;

public sealed class FakeCoordinatesAgent : ICoordinatesAgent
{
    // TODO: Fake Agent Service, dummy implementations, replace with Google API; since 3rd party agent, handle exceptions and timeouts
    public Task<Maybe<string>> GetCityByCoordinatesAsync(Coordinates coordinates, CancellationToken cancellationToken = default)
    {
        Guards.ThrowIfNull(coordinates);

        if (coordinates.Latitude > 0)
        {
            return Task.FromResult((Maybe<string>)"Barcelona");
        }

        return Task.FromResult((Maybe<string>)"Sabadell");
    }

    public Task<Maybe<string>> GetLocationByCoordinatesAsync(Coordinates coordinates, CancellationToken cancellationToken = default)
    {
        Guards.ThrowIfNull(coordinates);

        if (coordinates.Latitude > 0)
        {
            return Task.FromResult((Maybe<string>)"Barcelona");
        }

        return Task.FromResult((Maybe<string>)"Sabadell");
    }
}
