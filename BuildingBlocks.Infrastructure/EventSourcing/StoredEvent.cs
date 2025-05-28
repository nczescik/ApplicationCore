namespace BuildingBlocks.Infrastructure.EventSourcing
{
    public class StoredEvent
    {
        public Guid Id { get; set; } 
        public Guid AggregateId { get; set; }
        public string Type { get; set; } = default!;
        public string Data { get; set; } = default!;
        public DateTime OccurredOn { get; set; }
    }
}
