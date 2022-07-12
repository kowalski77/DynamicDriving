using DynamicDriving.SharedKernel;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Services;

public class TripCostCalculator : ITripCostCalculator
{
    // NOTE: This is a stub implementation, simulating a complex system
    public int CalculateCost(Location origin, Location destination)
    {
        Guards.ThrowIfNull(origin);
        Guards.ThrowIfNull(destination);
        
        var random = new Random();
        var cost = random.Next(1, 10);

        return cost;
    }
}
