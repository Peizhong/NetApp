using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetApp.Entities.Mall;
using NetApp.Repository.Interfaces;
using Dapper;
using MySql.Data.MySqlClient;
using System.Linq.Expressions;

namespace NetApp.Repository
{
    public class MQMallRepo : IListRepo<Product>
    {
        public Task<Product> FindAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetAsync(Expression expression)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Product>> GetListAsync(Expression expression)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Product>> ProductsAsync(int startIndex, int pageSize)
        {
            using (var conn = new MySqlConnection(""))
            {
                var query = await conn.QueryAsync<Product>("SELECT id,title,text,link,date_add as updatetime,topic_id as topicid FROM learning_logs_entry;");
                var res = query.ToList();
                return res;
            }
        }

        public Task UpdateAsync(Product value)
        {
            throw new NotImplementedException();
        }

        public Task UpdateListAsync(IEnumerable<Product> values)
        {
            throw new NotImplementedException();
        }
    }
}
