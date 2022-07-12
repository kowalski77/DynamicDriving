using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Services;

public sealed class TripService : ITripService
{
    private readonly ILocationFactory locationFactory;
    private readonly ITripValidator tripValidator;
    private readonly ITripCostCalculator tripCostCalculator;

    public TripService(ILocationFactory locationFactory, ITripValidator tripValidator, ITripCostCalculator tripCostCalculator)
    {
        this.locationFactory = Guards.ThrowIfNull(locationFactory);
        this.tripValidator = Guards.ThrowIfNull(tripValidator);
        this.tripCostCalculator = tripCostCalculator;
    }

    public async Task<Result<Trip>> CreateDraftTripAsync(
        UserId userId, DateTime pickUp,
        Coordinates origin, Coordinates destination,
        CancellationToken cancellationToken = default)
    {
        Guards.ThrowIfNull(userId);
        Guards.ThrowIfNull(origin);
        Guards.ThrowIfNull(destination);

        var result = await Result.Init
            .OnSuccess(async () => await this.tripValidator.ValidateTripDistanceAsync(origin, destination, cancellationToken))
            .OnSuccess(async () => await this.CreateLocationsAsync(origin, destination, cancellationToken))
            .OnSuccess(locations => new Trip(userId, pickUp, locations.Item1, locations.Item2))
            .OnSuccess(trip =>
            {
                var cost = this.tripCostCalculator.CalculateCost(trip.Origin, trip.Destination);
                trip.SetCost(cost);

                return trip;
            });

        return result;
    }

    private async Task<Result<(Location, Location)>> CreateLocationsAsync(Coordinates origin, Coordinates destination, CancellationToken cancellationToken)
    {
        var originLocation = await this.locationFactory.CreateAsync(origin, cancellationToken);
        if (originLocation.Failure)
        {
            return originLocation.Error!;
        }

        var destinationLocation = await this.locationFactory.CreateAsync(destination, cancellationToken);
        if (destinationLocation.Failure)
        {
            return destinationLocation.Error!;
        }

        return (originLocation.Value, destinationLocation.Value);
    }
}
