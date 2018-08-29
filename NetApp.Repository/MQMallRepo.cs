using NetApp.Common.Interfaces;
using NetApp.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetApp.Repository
{
    public class MQMallRepo : EFRepo, IListRepo<Product>, ITreeRepo<Category>
    {
        public MQMallRepo(string connectionString)
        {
            _absContext = new MallDbContext(connectionString);

            //_context.Database.EnsureDeleted();
            //_context.Database.EnsureCreated();
            //_context.Database.Migrate();
        }

        async Task<Product> IListRepo<Product>.FindAsync(string id)
        {
            var result = await getByIdAsync<Product>(id);
            return result;
        }

        async Task<Category> IListRepo<Category>.FindAsync(string id)
        {
            var result = await getByIdAsync<Category>(id);
            return result;
        }

        public async Task<Product> GetAsync(IQuery<Product> query)
        {
            Product product = await getByQueryAsync(query);
            return product;
        }

        public async Task<Category> GetAsync(IQuery<Category> query)
        {
            Category categpory = await getByQueryAsync(query);
            return categpory;
        }

        public async Task<PageableQueryResult<Product>> GetListAsync(PageableQuery<Product> query)
        {
            var products = await getListByQueryAsync(query);
            if (products.Items?.Count > 0)
            {
                //add category info to product
                var categoryIds = products.Items.Select(p => p.CategoryId).ToList();
                var categories = await getListByFilterAsync<Category>(c => categoryIds.Contains(c.Id));
            }
            return products;
        }

        public async Task<PageableQueryResult<Category>> GetListAsync(PageableQuery<Category> query)
        {
            var categories = await getListByQueryAsync(query);
            return categories;
        }

        public Task UpdateAsync(Product value)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Category value)
        {
            throw new NotImplementedException();
        }

        public Task UpdateListAsync(IEnumerable<Product> values)
        {
            throw new NotImplementedException();
        }

        public Task UpdateListAsync(IEnumerable<Category> values)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Category>> GetAllChildren(string id)
        {
            throw new NotImplementedException();
        }
    }
}