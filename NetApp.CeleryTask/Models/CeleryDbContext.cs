using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.CeleryTask.Models
{
    public class CeleryDbContext : DbContext
    {
        public CeleryDbContext(DbContextOptions<CeleryDbContext> options) :
            base(options)
        {

        }

        public DbSet<PeriodicTask> PeriodicTasks { get; set; }
    }
}
