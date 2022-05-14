using DynamicDriving.SharedKernel.Mongo;

namespace DynamicDriving.DriverManagement.Core.Trips;

public interface ITripRepository : IMongoRepository<Trip>
{
}
