using AuthService.Domain.Users;
using BuildingBlocks.Infrastructure.EventSourcing;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Messaging.Outbox;
using Shared.Infrastructure.Messaging.Outbox;

namespace AuthService.Infrastructure.Persistance
{
    public class AuthDbContext : EventStoreDbContext, IOutboxDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }
    }
}
