
namespace BuildingBlocks.Domain.Common
{
    public abstract class AggregateRoot : Entity
    {
        private readonly List<IDomainEvent> _uncommittedEvents = new();

        protected AggregateRoot() : base() { }

        public IReadOnlyCollection<IDomainEvent> UncommittedEvents => _uncommittedEvents.AsReadOnly();

        public void MarkEventsAsCommitted() => _uncommittedEvents.Clear();

        protected void RaiseEvent(IDomainEvent @event)
        {
            Apply(@event);
            _uncommittedEvents.Add(@event);
        }

        public void LoadFromHistory(IEnumerable<IDomainEvent> history)
        {
            foreach (var e in history)
            {
                Apply(e);
            }
        }

        protected abstract void Apply(IDomainEvent @event);
    }
}