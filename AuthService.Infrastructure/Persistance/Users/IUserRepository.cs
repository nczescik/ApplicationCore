﻿using AuthService.Domain.Users;

namespace AuthService.Infrastructure.Persistance.Users
{
    public interface IUserRepository
    {
        Task SaveAsync(User user, CancellationToken cancellationToken);

        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
