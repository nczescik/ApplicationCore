using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Infrastructure.EventSourcing
{
    public class EventStoreDbContext : DbContext
    {
        public DbSet<StoredEvent> Events => Set<StoredEvent>();

        public EventStoreDbContext(DbContextOptions<EventStoreDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StoredEvent>().HasKey(e => e.Id);
        }
    }
}
