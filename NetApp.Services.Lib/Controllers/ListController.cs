using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using NetApp.Models.Interfaces;
using NetApp.Repository.Interfaces;

namespace NetApp.Services.Lib.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public abstract class ListController<T> : CacheController where T : IBase
    {
        private readonly IListRepo<T> _repo;

        public ListController(ILogger<ListController<T>> logger, IDistributedCache cache, IListRepo<T> repo)
            : base(logger, cache)
        {
            _repo = repo;
        }

        /// <summary>
        /// 查询集合，url不带参数，写在header中
        /// 响应中包含self,next,prev的链接和total
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageableQueryResult<T>> GetAsync()
        {
            var page = new PageableQuery<T>();
            Func<Task<PageableQueryResult<T>>> query = async () =>
            {
                page.Filter = (t) => t.DataStatus != 2;
                return await _repo.GetListAsync(page);
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