using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using NetApp.EventBus;
using NetApp.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetApp.Services.Catalog
{
    public class DesignTimeEventLogDbContextFactory :
        IDesignTimeDbContextFactory<IntegrationEventLogContext>
    {
        private readonly IConfiguration _configuration;
        public DesignTimeEventLogDbContextFactory()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }
        
        IntegrationEventLogContext IDesignTimeDbContextFactory<IntegrationEventLogContext>.CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<IntegrationEventLogContext>();
            var connStr = _configuration.GetConnectionString("MallDB");
            builder.UseMySql(connStr, b => b.MigrationsAssembly("NetApp.Services.Catalog"));
            return new IntegrationEventLogContext(builder.Options);
        }
    }
}