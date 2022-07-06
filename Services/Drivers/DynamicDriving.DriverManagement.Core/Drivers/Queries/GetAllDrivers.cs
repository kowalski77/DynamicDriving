using DynamicDriving.SharedKernel.Application;
using MongoDB.Driver;

namespace DynamicDriving.DriverManagement.Core.Drivers.Queries;

public record GetAllDrivers() : ICommand<IReadOnlyList<DriverSummaryDto>>;

public class GetAllDriversServiceCommand : IServiceCommand<GetAllDrivers, IReadOnlyList<DriverSummaryDto>>
{
    private readonly IDriverRepository driverRepository;

    public GetAllDriversServiceCommand(IDriverRepository driverRepository)
    {
        this.driverRepository = driverRepository;
    }

    public async Task<IReadOnlyList<DriverSummaryDto>> ExecuteAsync(GetAllDrivers command, CancellationToken cancellationToken = default)
    {
        var options = new FindOptions<Driver, DriverSummaryDto>
        {
            Projection = Builders<Driver>.Projection.Expression(x => new DriverSummaryDto(x.Id, x.Name, x.Car.Model, x.IsAvailable))
        };

        var driverSummaries = await this.driverRepository.GetAllAsync(_ => true, options, cancellationToken).ConfigureAwait(false);

        return driverSummaries;
    }
}
