using Microsoft.EntityFrameworkCore;
using NetApp.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.EventBus
{
    public class IntegrationEventLogContext : DbContext
    {
        public IntegrationEventLogContext(DbContextOptions<IntegrationEventLogContext> options) : base(options)
        {
        }

        public DbSet<IntegrationEventLogEntry> IntegrationEventLogs { get; set; }
    }
}
