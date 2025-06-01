namespace AuthService.Application.CQRS.User
{
    public interface IUserRepository
    {
        Task SaveAsync(Domain.Users.User user, CancellationToken cancellationToken);
        Task<Domain.Users.User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Domain.Users.User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken);
    }
}
