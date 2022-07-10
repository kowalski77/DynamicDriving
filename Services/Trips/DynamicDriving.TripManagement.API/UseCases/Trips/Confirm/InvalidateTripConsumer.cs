using DynamicDriving.SharedKernel;
using MassTransit;
using MediatR;
using InvalidateTrip = DynamicDriving.Contracts.Trips.InvalidateTrip;
using InvalidateTripCommand = DynamicDriving.TripManagement.Application.Trips.Commands.InvalidateTrip;

namespace DynamicDriving.TripManagement.API.UseCases.Trips.Confirm;

public class InvalidateTripConsumer : IConsumer<InvalidateTrip>
{
    private readonly IMediator mediator;

    public InvalidateTripConsumer(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task Consume(ConsumeContext<InvalidateTrip> context)
    {
        Guards.ThrowIfNull(context);

        var command = new InvalidateTripCommand(context.Message.TripId, context.Message.CorrelationId);

        await this.mediator.Send(command).ConfigureAwait(false);
    }
}
