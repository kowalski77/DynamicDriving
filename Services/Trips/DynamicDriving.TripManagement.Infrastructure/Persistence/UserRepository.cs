using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.TripManagement.Domain.UsersAggregate;

namespace DynamicDriving.TripManagement.Infrastructure.Persistence;

public sealed class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(TransactionContext context) : base(context)
    {
    }
}
