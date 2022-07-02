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

    public async Task<IReadOnlyList<DriverSummaryDto>> GetDriversSummaryAsync(CancellationToken cancellationToken)
    {
        var options = new FindOptions<Driver, DriverSummaryDto>
        {
            Projection = Builders<Driver>.Projection.Expression(x => new DriverSummaryDto(x.Id, x.Name, x.Car.Model, x.IsAvailable))
        };

        var driverdummaries = await this.GetAllAsync(_ => true, options, cancellationToken).ConfigureAwait(false);

        return driverdummaries;
    }
}
