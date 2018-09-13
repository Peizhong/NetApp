using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetApp.Model
{
    public class NetAppDbContext : DbContext
    {
        public NetAppDbContext(DbContextOptions<NetAppDbContext> options) :
            base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }
    }
}
