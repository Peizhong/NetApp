using Dapper;
using NetApp.PlayASP.Abstract;
using NetApp.PlayASP.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NetApp.PlayASP.Concrete
{
    public class DPProductRepository : IProductRepository
    {
        private readonly string ConnectionString;

        public DPProductRepository()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public IEnumerable<Product> Products()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var result = conn.Query<Product>("select * from Products");
                if (!result.Any())
                {
                    var demo = Enumerable.Range(1, 10).Select(n => new Product
                    {
                        Name = $"产品{n}",
                        Price = n,
                        Category = $"类别{n % 2}",
                        Description = "hahaha"
                    });
                    conn.Execute(@"insert Products(Name,Price,Category,Description) values (@Name, @Price, @Category, @Description)", demo);
                }
                return result;
            }
        }
    }
}