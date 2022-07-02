using MediatR;

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
        var driverSummaries = await this.driverRepository.GetDriversSummaryAsync(cancellationToken).ConfigureAwait(false);

        return driverSummaries;
    }
}
