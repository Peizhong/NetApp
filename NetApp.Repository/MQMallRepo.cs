using Microsoft.EntityFrameworkCore.Infrastructure;
using NetApp.Common.Abstractions;
using NetApp.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetApp.Repository
{
    public class MQMallRepo : EFRepo, IListRepo<Product>, ITreeRepo<Category>
    {
        public MQMallRepo(MallDBContext _mallDbContext)
        {
            _absContext = _mallDbContext;
        }

        public DatabaseFacade Database => _absContext.Database;

        public Task SaveChangesAsync()
        {
            return _absContext.SaveChangesAsync();
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

        public async Task UpdateAsync(Product value, RepoOption option = null)
        {
            var entity = await _absContext.Set<Product>().FindAsync(value.Id);
            _absContext.Entry(entity).CurrentValues.SetValues(value);
            if (option?.SaveLater == true)
                return;
            await _absContext.SaveChangesAsync();
        }

        public Task UpdateAsync(Category value, RepoOption option = null)
        {
            throw new NotImplementedException();
        }

        public Task UpdateListAsync(IEnumerable<Product> values, RepoOption option)
        {
            throw new NotImplementedException();
        }

        public Task UpdateListAsync(IEnumerable<Category> values, RepoOption option)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Category>> GetAllChildren(string id)
        {
            throw new NotImplementedException();
        }
    }
}