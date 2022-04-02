using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.TripManagement.Domain.UsersAggregate;

namespace DynamicDriving.TripManagement.Infrastructure.Persistence;

public sealed class UserRepository : IUserRepository
{
    public IUnitOfWork UnitOfWork { get; }

    public User Add(User item)
    {
        throw new NotImplementedException();
    }

    public Task<Maybe<User>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
