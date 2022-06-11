using DynamicDriving.DriverManagement.Core.Drivers;
using DynamicDriving.DriverManagement.Core.Outbox;
using DynamicDriving.Events;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.Mediator;
using DynamicDriving.SharedKernel.Results;

namespace DynamicDriving.DriverManagement.Core.Trips.Commands;

public sealed record AssignDriver(Guid TripId) : ICommand<Result<AssignDriverDto>>;

public sealed class AssignDriverHandler : ICommandHandler<AssignDriver, Result<AssignDriverDto>>
{
    private readonly IOutboxService outboxService;
    private readonly ITripRepository tripRepository;
    private readonly IDriverService driverService;

    public AssignDriverHandler(ITripRepository tripRepository, IDriverService driverService, IOutboxService outboxService)
    {
        this.driverService = Guards.ThrowIfNull(driverService);
        this.tripRepository = Guards.ThrowIfNull(tripRepository);
        this.outboxService = Guards.ThrowIfNull(outboxService);
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

        var updatedTrip = trip.With(driver);
        await this.tripRepository.UpdateAsync(updatedTrip, cancellationToken);

        var driverAssigned = new DriverAssigned(Guid.NewGuid(), request.TripId, driver.Id);
        await this.outboxService.PublishIntegrationEventAsync(driverAssigned, cancellationToken);

        return Result.Ok(new AssignDriverDto(trip.Id, driver.Id));
    }
}
