using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Shared.Infrastructure.Messaging.RabbitMQ
{
    public class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly IConnectionFactory _factory;

        public RabbitMqEventPublisher(IConnectionFactory factory)
        {
            _factory = factory;
        }

        public void Publish<T>(T @event)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            var queue = typeof(T).Name;
            channel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: false);

            var json = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: queue, body: body);
        }
    }

}
