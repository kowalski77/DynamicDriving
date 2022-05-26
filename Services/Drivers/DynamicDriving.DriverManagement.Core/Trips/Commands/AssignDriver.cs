using DynamicDriving.DriverManagement.Core.Drivers;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.Mediator;
using DynamicDriving.SharedKernel.Results;

namespace DynamicDriving.DriverManagement.Core.Trips.Commands;

public sealed record AssignDriver(Guid TripId) : ICommand<Result<AssignDriverDto>>;

public sealed class AssignDriverHandler : ICommandHandler<AssignDriver, Result<AssignDriverDto>>
{
    private readonly ITripRepository tripRepository;
    private readonly IDriverService driverService;

    public AssignDriverHandler(ITripRepository tripRepository, IDriverService driverService)
    {
        this.driverService = Guards.ThrowIfNull(driverService);
        this.tripRepository = Guards.ThrowIfNull(tripRepository);
    }

    public async Task<Result<AssignDriverDto>> Handle(AssignDriver request, CancellationToken cancellationToken)
    {
        Guards.ThrowIfNull(request);

        var trip = await this.tripRepository.GetAsync(request.TripId, cancellationToken);
        if (trip is null)
        {
            return GeneralErrors.NotFound(request.TripId, nameof(Trip));
        }

        var canAssignResult = trip.CanAssignDriver();
        if (canAssignResult.Failure)
        {
            return canAssignResult.Error!;
        }

        var driver = await this.driverService.GetFirstAvailableDriverAsync(cancellationToken);
        if (driver is null)
        {
            return TripErrors.NoDriverAvailable();
        }

        trip.Assign(driver);
        await this.tripRepository.UpdateAsync(trip, cancellationToken);

        return Result.Ok(new AssignDriverDto(trip.Id, driver.Id));
    }
}
