namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Services;

public interface ITripCostCalculator
{
    int CalculateCost(Location origin, Location destination);
}