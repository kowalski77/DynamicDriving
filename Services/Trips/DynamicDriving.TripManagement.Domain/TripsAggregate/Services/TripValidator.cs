using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Services;

public class TripValidator : ITripValidator
{
    private const decimal MinimumDistanceBetweenLocations = 1;
    private readonly ICoordinatesAgent coordinatesAgent;

    public TripValidator(ICoordinatesAgent coordinatesAgent)
    {
        this.coordinatesAgent = Guards.ThrowIfNull(coordinatesAgent);
    }

    public async Task<Result> ValidateTripDistanceAsync(Coordinates origin, Coordinates destination, CancellationToken cancellationToken = default)
    {
        var kilometers = await this.coordinatesAgent.GetDistanceInKmBetweenCoordinates(origin, destination, cancellationToken);

        return kilometers < MinimumDistanceBetweenLocations ? 
            TripErrors.MinimumDistanceBetweenLocations(MinimumDistanceBetweenLocations) : 
            Result.Ok();
    }
}
