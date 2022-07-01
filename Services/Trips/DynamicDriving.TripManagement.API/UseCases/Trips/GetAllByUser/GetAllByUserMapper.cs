using DynamicDriving.Models;
using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Domain.TripsAggregate;

namespace DynamicDriving.TripManagement.API.UseCases.Trips.GetAllByUser;

public static class GetAllByUserMapper
{
    public static TripsByUserResponse AsResponse(this IReadOnlyList<TripSummaryDto> trips, Guid userId)
    {
        Guards.ThrowIfNull(trips);

        return new TripsByUserResponse(
            userId, 
            trips.Select(x => new TripSummary(
                x.DriverName, 
                x.Status, 
                x.Origin, 
                x.Destination)));
    }
}
