using MediatR;
using MongoDB.Driver;

namespace DynamicDriving.DriverManagement.Core.Drivers.Queries;

public record GetAllDrivers() : IRequest<IReadOnlyList<DriverSummaryDto>>;

public class GetAllDriversHandler : IRequestHandler<GetAllDrivers, IReadOnlyList<DriverSummaryDto>>
{
    private readonly IDriverRepository driverRepository;

    public GetAllDriversHandler(IDriverRepository driverRepository)
    {
        this.driverRepository = driverRepository;
    }

    public async Task<IReadOnlyList<DriverSummaryDto>> Handle(GetAllDrivers request, CancellationToken cancellationToken)
    {
        var options = new FindOptions<Driver, DriverSummaryDto>
        {
            Projection = Builders<Driver>.Projection.Expression(x => new DriverSummaryDto(x.Id, x.Name, x.Car.Model, x.IsAvailable))
        };

        var driverSummaries = await this.driverRepository.GetAllAsync(_ => true, options, cancellationToken).ConfigureAwait(false);

        return driverSummaries;
    }
}
