using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.DomainDriven;

namespace DynamicDriving.TripManagement.Domain.Common;

public class Coordinates : ValueObject
{
    private Coordinates(decimal latitude, decimal longitude)
    {
        this.Latitude = Guards.ThrowIfLessThan(latitude, 0);
        this.Longitude = Guards.ThrowIfLessThan(longitude, 0);
    }

    public decimal Latitude { get; }

    public decimal Longitude { get; }

    public static Coordinates CreateInstance(decimal latitude, decimal longitude)
    {
        return new Coordinates(latitude, longitude);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Latitude;
        yield return this.Longitude;
    }
}
