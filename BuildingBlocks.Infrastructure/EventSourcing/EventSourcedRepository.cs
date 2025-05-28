using BuildingBlocks.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace BuildingBlocks.Infrastructure.EventSourcing
{
    public abstract class EventSourcedRepository<TAggregate, TDbContext> : IEventSourcedRepository<TAggregate>
        where TAggregate : AggregateRoot, new()
        where TDbContext : EventStoreDbContext
    {
        private readonly EventStoreDbContext _dbContext;

        protected EventSourcedRepository(EventStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TAggregate?> GetByIdAsync(Guid id)
        {
            var events = await _dbContext.Events
                .Where(e => e.AggregateId == id)
                .OrderBy(e => e.OccurredOn)
                .ToListAsync();

            if (!events.Any()) return null;

            var aggregate = new TAggregate();

            var deserialized = events.Select(e =>
                (IDomainEvent)JsonConvert.DeserializeObject(e.Data, Type.GetType(e.Type)!)!
            );

            aggregate.LoadFromHistory(deserialized);
            return aggregate;
        }

        public async Task SaveAsync(TAggregate aggregate)
        {
            var newEvents = aggregate.UncommittedEvents.Select(e => new StoredEvent
            {
                Id = Guid.NewGuid(),
                AggregateId = aggregate.Id,
                Type = aggregate.GetType().AssemblyQualifiedName!,
                Data = JsonConvert.SerializeObject(e),
                OccurredOn = e.OccurredOn
            });

            await _dbContext.Events.AddRangeAsync(newEvents);
            await _dbContext.SaveChangesAsync();
            aggregate.MarkEventsAsCommitted();
        }
    }
}

