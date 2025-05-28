using BuildingBlocks.Infrastructure.EventSourcing;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Messaging.Outbox;

namespace AuthService.Infrastructure
{
    public class AuthDbContext : EventStoreDbContext, IOutboxDbContext
    {
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }
    }
}
