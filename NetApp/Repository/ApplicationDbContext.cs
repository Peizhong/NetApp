using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using NetApp.Entities;
using NetApp.Entities.LearningLog;
using NetApp.Entities.Avmt;

namespace NetApp.Repository
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Entry> Entries { get; set; }

        public DbSet<Topic> Topics { get; set; }

        public DbSet<NetApp.Entities.Avmt.Workspace> Workspace { get; set; }

        public DbSet<NetApp.Entities.Avmt.FunctionLocation> FunctionLocation { get; set; }
    }
}