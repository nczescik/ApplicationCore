namespace BuildingBlocks.Domain.Common
{
    public interface IEventSourcedRepository<TAggregate> where TAggregate : AggregateRoot
    {
        Task<TAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken);
    }
}
