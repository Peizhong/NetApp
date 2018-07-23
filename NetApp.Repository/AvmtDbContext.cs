using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NetApp.Entities.Avmt;

namespace NetApp.Repository
{
    public class AvmtDbContext : DbContext
    {
        public AvmtDbContext(DbContextOptions<AvmtDbContext> options)
            : base(options)
        {
        }

        public DbSet<Classify> Classifies { get; set; }
        public DbSet<BasicInfoConfig> BasicinfoConfigs { get; set; }
        public DbSet<BasicInfoDictConfig> BasicInfoDictConfigs { get; set; }
        public DbSet<TechparamConfig> TechparamConfigs { get; set; }
        public DbSet<TechparamDictConfig> TechparamDictConfigs { get; set; }

        public DbSet<MainTransferBill> MainTransferBills { get; set; }
        public DbSet<DisTransferBill> DisTransferBills { get; set; }
        public DbSet<ChangeBill> ChangeBills { get; set; }

        public DbSet<FunctionLocation> FunctionLocations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<FunctionLocation>().HasKey(f => new { f.Id, f.WorkspaceId });
        }
    }

}
