using Microsoft.EntityFrameworkCore;
using NetApp.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Repository
{
    public class BasketDBContext : DbContext
    {
        public BasketDBContext(DbContextOptions<BasketDBContext> options) :
            base(options)
        {
            ;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BasketCheckout>().HasKey(c => c.RequestId);
            modelBuilder.Entity<BasketItem>().HasKey(i => i.Id);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<BasketItem> BasketItems { get; set; }

        public DbSet<BasketCheckout> BasketCheckouts { get; set; }
    }
}
