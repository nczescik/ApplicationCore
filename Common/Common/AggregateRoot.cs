namespace BuildingBlocks.Domain.Common
{
    public abstract class AggregateRoot<Guid>
    {
        private readonly List<IEvent> _uncommittedEvents = new();

        public Guid Id { get; protected set; } = default!;

        public IReadOnlyCollection<IEvent> UncommittedEvents => _uncommittedEvents.AsReadOnly();

        public void MarkEventsAsCommitted() => _uncommittedEvents.Clear();

        protected void RaiseEvent(IEvent @event)
        {
            Apply(@event);
            _uncommittedEvents.Add(@event);
        }

        public void LoadFromHistory(IEnumerable<IEvent> history)
        {
            foreach (var e in history)
            {
                Apply(e);
            }
        }

        protected abstract void Apply(IEvent @event);
    }
}