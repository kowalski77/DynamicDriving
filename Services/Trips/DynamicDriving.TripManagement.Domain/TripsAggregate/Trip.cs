#pragma warning disable 8618
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.DriversAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Exceptions;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate;

public sealed class Trip : Entity, IAggregateRoot
{
    private Trip() { }

    public Trip(Guid id, UserId userId, DateTime pickUp, Location origin, Location destination)
    {
        this.Id = Guards.ThrowIfEmpty(id);
        this.UserId = Guards.ThrowIfNull(userId);
        this.PickUp = pickUp;
        this.Origin = Guards.ThrowIfNull(origin);
        this.Destination = Guards.ThrowIfNull(destination);
        this.CurrentCoordinates = origin.Coordinates;
        this.TripStatus = TripStatus.Draft;
        this.Kilometers = 0;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public UserId UserId { get; private set; }

    public Driver? Driver { get; private set; }

    public DateTime PickUp { get; private set; }

    public Location Origin { get; private set; }

    public Location Destination { get; private set; }

    public Coordinates CurrentCoordinates { get; private set; }

    public TripStatus TripStatus { get; private set; }

    public decimal Kilometers { get; private set; }

    public Result CanConfirm()
    {
        return this.TripStatus is TripStatus.Draft ?
            Result.Ok() :
            TripErrors.ConfirmFailed(this.TripStatus);
    }

    public void Confirm()
    {
        var result = this.CanConfirm();
        if (result.Failure)
        {
            throw new TripConfirmationException(result.Error!.Message);
        }

        this.TripStatus = TripStatus.Confirmed;
    }

    public Result CanInvalidate()
    {
        return this.TripStatus is TripStatus.Confirmed ?
            Result.Ok() :
            TripErrors.InvalidateFailed(this.TripStatus);
    }

    public void Invalidate()
    {
        var result = this.CanInvalidate();
        if (result.Failure)
        {
            throw new TripInvalidationException(result.Error!.Message);
        }

        this.TripStatus = TripStatus.Draft;
    }

    public Result CanAssignDriver()
    {
        return this.TripStatus is TripStatus.Draft or TripStatus.Confirmed ?
            Result.Ok() :
            TripErrors.DriverAssignedFailed(this.TripStatus);
    }

    public void Assign(Driver driver)
    {
        var result = this.CanAssignDriver();
        if (result.Failure)
        {
            throw new AssignDriverFailedException(result.Error!.Message);
        }

        this.Driver = driver;
    }
}
