using AuthService.Domain.Users;
using BuildingBlocks.Infrastructure.EventSourcing;

namespace AuthService.Infrastructure.Users
{
    public class UserRepository : EventSourcedRepository<User, AuthDbContext>, IUserRepository
    {
        public UserRepository(AuthDbContext dbContext) : base(dbContext)
        {
        }
    }
}
