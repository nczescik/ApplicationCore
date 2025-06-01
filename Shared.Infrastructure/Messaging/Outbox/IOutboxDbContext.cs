using Microsoft.EntityFrameworkCore;
using Shared.Application.Messaging.Outbox;

namespace Shared.Infrastructure.Messaging.Outbox
{
    public interface IOutboxDbContext
    {
        DbSet<OutboxMessage> OutboxMessages { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
