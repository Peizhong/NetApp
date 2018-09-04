using Microsoft.EntityFrameworkCore;
using NetApp.EventBus.Model;

namespace NetApp.EventBus
{
    public class IntegrationEventLogContext : DbContext
    {
        public IntegrationEventLogContext(DbContextOptions<IntegrationEventLogContext> options) : base(options)
        {
        }

        public DbSet<IntegrationEventLogEntry> IntegrationEventLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IntegrationEventLogEntry>()
                .HasKey(c => c.EventId);

            base.OnModelCreating(modelBuilder);
        }
    }
}