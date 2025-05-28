
namespace BuildingBlocks.Domain.Common
{
    public interface IEventSourcedRepository<TAggregate> where TAggregate : AggregateRoot
    {
        Task<TAggregate?> GetByIdAsync(Guid id);
        Task SaveAsync(TAggregate aggregate);
    }
}
