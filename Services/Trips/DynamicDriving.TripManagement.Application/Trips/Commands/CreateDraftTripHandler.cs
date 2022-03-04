using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Mediator;
using DynamicDriving.SharedKernel.ResultModels;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Commands;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Services;

namespace DynamicDriving.TripManagement.Application.Trips.Commands;

public class CreateDraftTripHandler : ICommandHandler<CreateDraftTrip, IResultModel<DraftTripDto>>
{
    private readonly ITripService tripService;
    private readonly ITripRepository tripRepository;

    public CreateDraftTripHandler(ITripService tripService, ITripRepository tripRepository)
    {
        this.tripService = Guards.ThrowIfNull(tripService);
        this.tripRepository = Guards.ThrowIfNull(tripRepository);
    }

    public async Task<IResultModel<DraftTripDto>> Handle(CreateDraftTrip request, CancellationToken cancellationToken)
    {
        //Guards.ThrowIfNull(request);

        //var resultModel = await ResultModel.Init()
        //    .OnSuccess(async () =>
        //    {
        //        var result = await this.tripService.CreateDraftTripAsync()
        //    })

        throw new NotImplementedException();
    }
}
