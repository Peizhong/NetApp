using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

using NetApp.Entities.LearningLog;

namespace NetApp.Tests.Repository
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions options)
            :base(options)
        {
        }
        
        public DbSet<Entry> Entries { get; set; }

        public DbSet<Topic> Topics { get; set; }
    }
}