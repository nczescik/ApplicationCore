namespace BuildingBlocks.Infrastructure.EventSourcing
{
    public class StoredEvent
    {
        public Guid Id { get; set; } 
        public Guid AggregateId { get; set; }
        public string AggregateType { get; set; } = string.Empty!;
        public string EventType { get; set; } = string.Empty!;
        public string Data { get; set; } = string.Empty!;
        public DateTime OccurredOn { get; set; }
    }
}
