using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.LocationsAggregate.Services;

public class CoordinatesValidator : ICoordinatesValidator
{
    private readonly ILocationRepository locationRepository;
    private readonly ILocationProvider locationProvider;

    public CoordinatesValidator(ILocationRepository locationRepository, ILocationProvider locationProvider)
    {
        this.locationRepository = Guards.ThrowIfNull(locationRepository);
        this.locationProvider = Guards.ThrowIfNull(locationProvider);
    }

    public Result Validate(Coordinates coordinates)
    {
        var maybeLocation = this.locationProvider.GetLocation(coordinates);
        if (!maybeLocation.TryGetValue(out var location))
        {
            return Result.Fail(new ErrorResult("", "Invalid coordinates"));
        }

        var currentLocations = this.locationRepository.GetLocations();
        return currentLocations.Contains(location) ? Result.Ok() : Result.Fail(new ErrorResult("", ""));
    }
}
