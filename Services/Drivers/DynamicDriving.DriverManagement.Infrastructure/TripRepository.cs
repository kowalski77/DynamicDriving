using DynamicDriving.DriverManagement.Core.Trips;
using DynamicDriving.SharedKernel.Mongo;
using MongoDB.Driver;

namespace DynamicDriving.DriverManagement.Infrastructure;

public class TripRepository : MongoRepository<Trip>, ITripRepository
{
    public TripRepository(IMongoDatabase database) : base(database)
    {
    }
}
