using DynamicDriving.DriverManagement.Core.Trips.Commands;
using DynamicDriving.Events;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Application;
using DynamicDriving.SharedKernel.Results;
using MassTransit;

namespace DynamicDriving.DriverManagement.API.UseCases.Trips.Create;

public class TripConfirmedConsumer : IConsumer<TripConfirmed>
{
    private readonly IServiceCommand<CreateTrip, Result> serviceCommand;

    public TripConfirmedConsumer(IServiceCommand<CreateTrip, Result> serviceCommand)
    {
        this.serviceCommand = serviceCommand;
    }

    public async Task Consume(ConsumeContext<TripConfirmed> context)
    {
        Guards.ThrowIfNull(context);

        var command = context.Message.AsCommand();

        await this.serviceCommand.ExecuteAsync(command).ConfigureAwait(false);
    }
}
