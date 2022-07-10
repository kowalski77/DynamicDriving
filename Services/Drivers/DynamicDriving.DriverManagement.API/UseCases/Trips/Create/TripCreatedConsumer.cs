using DynamicDriving.Contracts.Trips;
using DynamicDriving.DriverManagement.Core.Trips.Commands;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Application;
using DynamicDriving.SharedKernel.Results;
using MassTransit;

namespace DynamicDriving.DriverManagement.API.UseCases.Trips.Create;

public class TripCreatedConsumer : IConsumer<TripCreated>
{
    private readonly IServiceCommand<CreateTrip, Result> serviceCommand;

    public TripCreatedConsumer(IServiceCommand<CreateTrip, Result> serviceCommand)
    {
        this.serviceCommand = serviceCommand;
    }

    public async Task Consume(ConsumeContext<TripCreated> context)
    {
        Guards.ThrowIfNull(context);

        CreateTrip command = context.Message.AsCommand();

        await this.serviceCommand.ExecuteAsync(command).ConfigureAwait(false);
    }
}
