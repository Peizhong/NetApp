using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using NetApp.Entities.Interfaces;
using NetApp.Repository.Interfaces;
using System.Linq.Expressions;

namespace NetApp.Services.Lib.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ListController<T> : CacheController where T : IQuery
    {
        protected readonly IListRepo<T> _repo;

        public ListController(ILogger<ListController<T>> logger, IDistributedCache cache, IListRepo<T> repo)
            : base(logger, cache)
        {
            _repo = repo;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<IList<T>> GetAsync()
        {
            Func<Task<IList<T>>> query = async () =>
            {
                Expression<Func<T, bool>> show = (t) => t.DataStatus != 0;
                return await _repo.GetListAsync(show);
            };
            var result = await CacheIt(query);
            return result;
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<T> GetAsync(string id)
        {
            Func<Task<T>> query = async () =>
            {
                return await _repo.FindAsync(id);
            };
            var result = await CacheIt(query);
            return result;
        }

        // POST: api/Products
        [HttpPost]
        public Task PostAsync([FromBody] T value)
        {
            return _repo.UpdateAsync(value);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public Task PutAsync(int id, [FromBody] T value)
        {
            return _repo.UpdateAsync(value);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}