using AuthService.Application.CQRS.User;
using AuthService.Domain.Users;
using BuildingBlocks.Infrastructure.EventSourcing;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Persistance.Users
{
    public class UserRepository : EventSourcedRepository<User, AuthDbContext>, IUserRepository
    {
        private readonly AuthDbContext _dbContext;
        public UserRepository(AuthDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            return _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
        }
    }
}
