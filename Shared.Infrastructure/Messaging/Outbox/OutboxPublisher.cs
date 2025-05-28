using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Shared.Infrastructure.Messaging.RabbitMQ;

namespace Shared.Infrastructure.Messaging.Outbox
{
    public class OutboxPublisher<TContext> : BackgroundService
        where TContext : DbContext, IOutboxDbContext
    {
        private readonly IServiceProvider _serviceProvider;

        public OutboxPublisher(
            IServiceProvider serviceProvider
            )
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
                var eventPublisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();

                var messages = await dbContext.OutboxMessages
                    .Where(m => m.ProcessedOn == null)
                    .OrderBy(m => m.OccurredOn)
                    .Take(20)
                    .ToListAsync(stoppingToken);

                foreach (var message in messages)
                {
                    try
                    {
                        var eventType = Type.GetType(message.EventType) ?? throw new InvalidOperationException($"Cannot resolve event type {message.EventType}");
                        var @event = JsonConvert.DeserializeObject(message.Data, eventType);

                        eventPublisher.Publish(@event);

                        message.ProcessedOn = DateTime.UtcNow;

                    }
                    catch (Exception ex)
                    {
                    }
                }

                await dbContext.SaveChangesAsync(stoppingToken);

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
