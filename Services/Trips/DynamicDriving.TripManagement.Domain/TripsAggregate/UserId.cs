using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.Results;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate;

public class UserId : ValueObject
{
    private UserId(Guid value)
    {
        this.Value = Guards.ThrowIfEmpty(value);
    }

    public Guid Value { get; }

    public static Result<UserId> CreateInstance(Guid value)
    {
        return value == Guid.Empty ? 
            Result.Fail<UserId>(GeneralErrors.InternalServerError($"{nameof(UserId)} is not valid")) : 
            Result.Ok(new UserId(value));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Value;
    }
}
