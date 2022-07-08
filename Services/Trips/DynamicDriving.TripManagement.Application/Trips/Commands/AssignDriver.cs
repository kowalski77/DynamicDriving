using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Mediator;
using DynamicDriving.TripManagement.Domain.DriversAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using MediatR;

namespace DynamicDriving.TripManagement.Application.Trips.Commands;

public sealed record AssignDriver(Guid TripId, Guid DriverId) : ICommand<Unit>;

public sealed class AssignDriverHandler : ICommandHandler<AssignDriver, Unit>
{
    private readonly ITripRepository tripRepository;
    private readonly IDriverRepository driverRepository;

    public AssignDriverHandler(ITripRepository tripRepository, IDriverRepository driverRepository)
    {
        this.tripRepository = Guards.ThrowIfNull(tripRepository);
        this.driverRepository = Guards.ThrowIfNull(driverRepository);
    }

    public async Task<Unit> Handle(AssignDriver request, CancellationToken cancellationToken)
    {
        Guards.ThrowIfNull(request);

        var trip = await this.GetTripByIdAsync(request.TripId, cancellationToken);
        var driver = await this.GetDriverByIdAsync(request.DriverId, cancellationToken);

        trip.Assign(driver);

        await this.tripRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task<Trip> GetTripByIdAsync(Guid tripId, CancellationToken cancellationToken)
    {
        var trip = await this.tripRepository.GetAsync(tripId, cancellationToken);
        if (trip.HasNoValue)
        {
            throw new InvalidOperationException($"No trip was found with id: {tripId}");
        }

        return trip.Value;
    }

    private async Task<Driver> GetDriverByIdAsync(Guid driverId, CancellationToken cancellationToken)
    {
        var driver = await this.driverRepository.GetAsync(driverId, cancellationToken);
        if (driver.HasNoValue)
        {
            throw new InvalidOperationException($"No driver was found with id: {driverId}");
        }

        return driver.Value;
    }
}
