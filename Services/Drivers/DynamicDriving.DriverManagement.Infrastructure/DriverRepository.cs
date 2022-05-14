using DynamicDriving.DriverManagement.Core.Drivers;
using DynamicDriving.SharedKernel.Mongo;
using MongoDB.Driver;

namespace DynamicDriving.DriverManagement.Infrastructure;

public sealed class DriverRepository : MongoRepository<Driver>, IDriverRepository
{
    public DriverRepository(IMongoDatabase database)
        : base(database)
    {
    }
}
