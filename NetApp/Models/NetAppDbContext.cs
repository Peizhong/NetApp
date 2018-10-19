using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetApp.Common.Models;
using NetApp.Workflow.Models;

namespace NetApp.Models
{
    public class NetAppDbContext : DbContext
    {
        public NetAppDbContext(DbContextOptions<NetAppDbContext> options) :
            base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }

        public DbSet<WorkflowRef> WorkflowRefs { get; set; }

        public DbSet<NetApp.Common.Models.BasketItem> BasketItem { get; set; }

        public DbSet<NetApp.Workflow.Models.NodeEntity> NodeEntity { get; set; }
    }
}
