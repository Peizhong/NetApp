using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetApp.Entities.Mall;
using NetApp.Repository.Interfaces;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NetApp.Repository
{
    public class MQMallRepo : IListRepo<Product>, ITreeRepo<Category>
    {
        private readonly MallDbContext _context;
        
        public MQMallRepo(string connectionString)
        {
            _context = new MallDbContext(connectionString);

            //_context.Database.EnsureCreated();
            //_context.Database.Migrate();
        }

        public Task<Product> FindAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Category>> GetAllChildren(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> GetAsync(Expression<Func<Product, bool>> expression)
        {
            var result = await _context.Products.FirstOrDefaultAsync(expression);
            return result;
        }

        public Task<Category> GetAsync(Expression<Func<Category, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Category>> GetChildren(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Product>> GetListAsync(Expression<Func<Product, bool>> expression)
        {
            var products = await _context.Products.Where(expression).ToListAsync();
            var categoryIds = products.Select(p => p.CategoryId).ToList();
            var categories = await _context.Categories.Where(c => categoryIds.Contains(c.Id)).ToListAsync();
            return products;
        }

        public async Task<IList<Category>> GetListAsync(Expression<Func<Category, bool>> expression)
        {
            var result = await _context.Categories.ToListAsync();
            return result;
        }

        public Task<IList<Category>> GetRoot()
        {
            throw new NotImplementedException();
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

        Task<Category> IListRepo<Category>.FindAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}