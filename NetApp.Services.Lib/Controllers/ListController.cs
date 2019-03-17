using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using NetApp.Common.Abstractions;
using NetApp.Models.Abstractions;

namespace NetApp.Services.Lib.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public abstract class ListController<T> : CacheController where T : IBase, new()
    {
        protected readonly IListRepo<T> _repo;

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
        public async Task<PageableQueryResult<T>> GetAsync([FromQuery]Pageable q)
        {
            var page = new PageableQuery<T>
            {
                PageSize = q.PageSize > 0 ? q.PageSize : 50
            };
            Func<Task<PageableQueryResult<T>>> query = async () =>
            {
                page.Filter = (t) => t.DataStatus != 2;
                return await _repo.GetListAsync(page);
            };
            var result = await CacheIt(query);
            result.Source = $"{Request.Host}";
            return result;
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<T> GetByIdAsync(string id)
        {
            Func<Task<T>> query = async () =>
            {
                return await _repo.FindAsync(id);
            };
            var result = await CacheIt(query);
            return result;
        }
    }
}