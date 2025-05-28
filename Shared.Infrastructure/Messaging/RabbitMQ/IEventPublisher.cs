namespace Shared.Infrastructure.Messaging.RabbitMQ
{
    public interface IEventPublisher
    {
        void Publish<T>(T @event);
    }
}
