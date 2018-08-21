using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetApp.Repository.Interfaces;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NetApp.Models.Mall;
using NetApp.Models.Interfaces;

namespace NetApp.Repository
{
    public class MQMallRepo : IListRepo<Product>, ITreeRepo<Category>
    {
        private readonly MallDbContext _context;

        public MQMallRepo(string connectionString)
        {
            _context = new MallDbContext(connectionString);

            _context.Database.EnsureCreated();
            _context.Database.Migrate();
        }

        async Task<Product> IListRepo<Product>.FindAsync(string id)
        {
            var result = await _context.Products.FindAsync(id);
            return result;
        }

        async Task<Category> IListRepo<Category>.FindAsync(string id)
        {
            var result = await _context.Categories.FindAsync(id);
            return result;
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

        public async Task<IList<Product>> GetListAsync(Expression<Func<Product, bool>> expression, IPageable<Product> pageable)
        {
            IQueryable<Product> query = _context.Products;
            if (expression != null)
                query = query.Where(expression);

            if (pageable.StartIndex > 0)
                query = query.Skip(pageable.StartIndex);
            if (pageable.PageSize > 0)
                query = query.Take(pageable.PageSize);

            var products = await query.ToListAsync();
            var categoryIds = products.Select(p => p.CategoryId).ToList();
            var categories = await _context.Categories.Where(c => categoryIds.Contains(c.Id)).ToListAsync();
            return products;
        }

        public async Task<IList<Category>> GetListAsync(Expression<Func<Category, bool>> expression, IPageable<Category> pageable)
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
    }
}