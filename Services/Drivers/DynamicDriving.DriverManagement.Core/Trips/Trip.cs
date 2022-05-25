using DynamicDriving.DriverManagement.Core.Drivers;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.Mongo;
using DynamicDriving.SharedKernel.Results;

namespace DynamicDriving.DriverManagement.Core.Trips;

public class Trip : IEntity
{
    public Trip(Guid id, DateTime pickUp, Coordinates origin, Coordinates destination)
    {
        this.Id = id;
        this.Origin = Guards.ThrowIfNull(origin);
        this.Destination = Guards.ThrowIfNull(destination);
        this.PickUp = pickUp;
        this.TripStatus = TripStatus.Unassigned;
    }

    public Coordinates Origin { get; private set; }

    public Coordinates Destination { get; private set;}

    public DateTime PickUp { get; private set;}

    public TripStatus TripStatus { get; private set;}

    public Driver? Driver { get; private set; }

    public Guid Id { get; private set; }

    public Result CanAssignDriver()
    {
        return this.TripStatus != TripStatus.Unassigned ? 
            new ErrorResult("", ""): 
            Result.Ok();
    }

    public void Assign(Driver driver)
    {
        Guards.ThrowIfNull(driver);

        var result = this.CanAssignDriver();
        if (result.Failure)
        {
            throw new InvalidOperationException(result.Error!.Message);
        }

        this.Driver = driver;
    }
}
