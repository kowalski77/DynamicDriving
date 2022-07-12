using DynamicDriving.SharedKernel.Mongo;

namespace DynamicDriving.TripService.API.Entities;

public class Trip : IEntity
{
    public Trip(Guid id, int credits)
    {
        this.Id = id;
        this.Credits = credits;
    }

    public Guid Id { get; private set; }

    public int Credits { get; private set; }
}
