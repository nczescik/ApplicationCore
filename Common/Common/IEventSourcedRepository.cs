
namespace BuildingBlocks.Domain.Common
{
    public interface IEventSourcedRepository<TAggregate, Guid> where TAggregate : AggregateRoot<Guid>
    {
        Task<TAggregate?> GetByIdAsync(Guid id);
        Task SaveAsync(TAggregate aggregate);
    }
}
