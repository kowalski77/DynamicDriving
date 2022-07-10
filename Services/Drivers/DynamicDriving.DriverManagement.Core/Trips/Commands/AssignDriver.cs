using DynamicDriving.Contracts.Drivers;
using DynamicDriving.DriverManagement.Core.Drivers;
using DynamicDriving.DriverManagement.Core.Outbox;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Application;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.Results;

namespace DynamicDriving.DriverManagement.Core.Trips.Commands;

public sealed record AssignDriver(Guid TripId) : ICommand<Result<AssignDriverDto>>;

public sealed class AssignDriverServiceCommand : IServiceCommand<AssignDriver, Result<AssignDriverDto>>
{
    private readonly IOutboxService outboxService;
    private readonly ITripRepository tripRepository;
    private readonly IDriverService driverService;

    public AssignDriverServiceCommand(ITripRepository tripRepository, IDriverService driverService, IOutboxService outboxService)
    {
        this.driverService = Guards.ThrowIfNull(driverService);
        this.tripRepository = Guards.ThrowIfNull(tripRepository);
        this.outboxService = Guards.ThrowIfNull(outboxService);
    }

    public async Task<Result<AssignDriverDto>> ExecuteAsync(AssignDriver command, CancellationToken cancellationToken = default)
    {
        Guards.ThrowIfNull(command);

        var trip = await this.tripRepository.GetAsync(command.TripId, cancellationToken).ConfigureAwait(false);
        if (trip is null)
        {
            return GeneralErrors.NotFound(command.TripId, nameof(Trip));
        }

        var canAssignResult = trip.CanAssignDriver();
        if (canAssignResult.Failure)
        {
            return canAssignResult.Error!;
        }

        var driver = await this.driverService.GetFirstAvailableDriverAsync(cancellationToken).ConfigureAwait(false);
        if (driver is null)
        {
            return TripErrors.NoDriverAvailable();
        }

        var updatedTrip = trip.With(driver);
        await this.tripRepository.UpdateAsync(updatedTrip, cancellationToken).ConfigureAwait(false);

        var driverAssigned = new DriverAssigned(command.TripId, driver.Id);
        await this.outboxService.PublishIntegrationEventAsync(driverAssigned, cancellationToken).ConfigureAwait(false);

        return Result.Ok(new AssignDriverDto(trip.Id, driver.Id));
    }
}
