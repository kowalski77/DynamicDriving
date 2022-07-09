using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Application;
using DynamicDriving.SharedKernel.Results;

namespace DynamicDriving.DriverManagement.Core.Trips.Commands;

public record InvalidateTrip(Guid TripId) : ICommand<Result>;

public sealed record InvalidateTripCommand : IServiceCommand<InvalidateTrip, Result>
{
    private readonly ITripRepository tripRepository;

    public InvalidateTripCommand(ITripRepository tripRepository)
    {
        this.tripRepository = tripRepository;
    }

    public async Task<Result> ExecuteAsync(InvalidateTrip command, CancellationToken cancellationToken = default)
    {
        Guards.ThrowIfNull(command);

        await this.tripRepository.RemoveAsync(command.TripId, cancellationToken).ConfigureAwait(false);

        return Result.Ok();
    }
}
