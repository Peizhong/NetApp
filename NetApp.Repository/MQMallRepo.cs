using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetApp.Entities.Mall;
using NetApp.Repository.Interfaces;
using Dapper;
using MySql.Data.MySqlClient;

namespace NetApp.Repository
{
    public class MQMallRepo : IMallRepo
    {
        //string connStr = "server=193.112.41.28;user=root;database=MYDEV;port=3306;password=mypass;SslMode=none";
        readonly string _connectionString;

        public MQMallRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task AddOrdersAsync(IEnumerable<Order> orders)
        {
            throw new NotImplementedException();
        }

        public Task AddProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task AddProductsAsync(IEnumerable<Product> products)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> OrdersAsync(string userId, int startIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<Product> ProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Product>> ProductsAsync(int startIndex, int pageSize)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                var query = await conn.QueryAsync<Product>("SELECT id,title,text,link,date_add as updatetime,topic_id as topicid FROM learning_logs_entry;");
                var res = query.ToList();
                return res;
            }
        }

        public Task RemoveProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
