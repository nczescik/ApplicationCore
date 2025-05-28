using BuildingBlocks.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BuildingBlocks.Infrastructure.EventSourcing
{

    namespace AuthService.Infrastructure.Persistence
    {
        public abstract class EventSourcedRepository<TAggregate> : IEventSourcedRepository<TAggregate>
            where TAggregate : AggregateRoot, new()
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
                    (IDomainEvent)JsonSerializer.Deserialize(e.Data, Type.GetType(e.Type)!)!
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
                    Type = e.GetType().AssemblyQualifiedName!,
                    Data = JsonSerializer.Serialize(e),
                    OccurredOn = e.OccurredOn
                });

                await _dbContext.Events.AddRangeAsync(newEvents);
                await _dbContext.SaveChangesAsync();
                aggregate.MarkEventsAsCommitted();
            }
        }
    }

}
