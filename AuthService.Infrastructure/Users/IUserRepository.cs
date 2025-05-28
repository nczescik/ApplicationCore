using AuthService.Domain.Users;

namespace AuthService.Infrastructure.Users
{
    public interface IUserRepository
    {
        Task SaveAsync(User user);

        Task<User?> GetByIdAsync(Guid id);
    }
}
