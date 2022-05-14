using DynamicDriving.SharedKernel.Mongo;

namespace DynamicDriving.DriverManagement.Core.Drivers;

public interface IDriverRepository : IMongoRepository<Driver>
{
}
