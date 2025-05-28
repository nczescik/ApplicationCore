using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Infrastructure.EventSourcing
{
    public abstract class EventStoreDbContext : DbContext
    {
        public DbSet<StoredEvent> Events => Set<StoredEvent>();

        public EventStoreDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StoredEvent>(builder =>
            {
                builder.HasKey(e => e.Id);
                builder.Property(e => e.AggregateId).IsRequired();
                builder.Property(e => e.AggregateType).IsRequired();
                builder.Property(e => e.EventType).IsRequired();
                builder.Property(e => e.Data).IsRequired();
                builder.Property(e => e.OccurredOn).IsRequired();
            });
        }
    }
}
