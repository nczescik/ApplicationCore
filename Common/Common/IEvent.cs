namespace BuildingBlocks.Domain.Common
{
    public interface IEvent
    {
        DateTime OccurredOn { get; }
    }
}