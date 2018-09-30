using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Workflow.Models
{
    public class WorkflowDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=workflow.db");
        }

        public DbSet<Flow> Flows { get; set; }
    }
}
