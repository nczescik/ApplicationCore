using Microsoft.EntityFrameworkCore;
using Shared.Application.Messaging.Outbox;

namespace Shared.Infrastructure.Messaging.Outbox.Repository
{
    public class OutboxRepository : IOutboxRepository
    {
        private readonly IOutboxDbContext _context;

        public OutboxRepository(IOutboxDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OutboxMessage message, CancellationToken cancellationToken = default)
        {
            await _context.OutboxMessages.AddAsync(message, cancellationToken);
        }

        public async Task<List<OutboxMessage>> GetUnprocessedAsync(CancellationToken cancellationToken = default)
        {
            return await _context.OutboxMessages
                .Where(m => m.ProcessedOn == null)
                .OrderBy(m => m.OccurredOn)
                .ToListAsync(cancellationToken);
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
