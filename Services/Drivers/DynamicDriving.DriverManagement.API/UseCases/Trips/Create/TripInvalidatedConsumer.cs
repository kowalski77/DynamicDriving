using DynamicDriving.Contracts.Trips;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Application;
using DynamicDriving.SharedKernel.Results;
using MassTransit;
using InvalidateTrip = DynamicDriving.DriverManagement.Core.Trips.Commands.InvalidateTrip;

namespace DynamicDriving.DriverManagement.API.UseCases.Trips.Create;

public class TripInvalidatedConsumer : IConsumer<TripInvalidated>
{
    private readonly IServiceCommand<InvalidateTrip, Result> serviceCommand;

    public TripInvalidatedConsumer(IServiceCommand<InvalidateTrip, Result> serviceCommand)
    {
        this.serviceCommand = serviceCommand;
    }

    public async Task Consume(ConsumeContext<TripInvalidated> context)
    {
        Guards.ThrowIfNull(context);

        var command = new InvalidateTrip(context.Message.TripId);

        await this.serviceCommand.ExecuteAsync(command).ConfigureAwait(false);
    }
}
