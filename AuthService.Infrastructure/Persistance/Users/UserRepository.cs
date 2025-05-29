using AuthService.Domain.Users;
using AuthService.Infrastructure.Persistance;
using BuildingBlocks.Infrastructure.EventSourcing;

namespace AuthService.Infrastructure.Persistance.Users
{
    public class UserRepository : EventSourcedRepository<User, AuthDbContext>, IUserRepository
    {
        public UserRepository(AuthDbContext dbContext) : base(dbContext)
        {
        }
    }
}
