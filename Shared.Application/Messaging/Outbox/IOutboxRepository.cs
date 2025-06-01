namespace Shared.Application.Messaging.Outbox
{
    public interface IOutboxRepository
    {
        Task AddAsync(OutboxMessage message, CancellationToken cancellationToken = default);
        Task<List<OutboxMessage>> GetUnprocessedAsync(CancellationToken cancellationToken = default);
        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}