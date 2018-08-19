using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Mvc;

namespace NetApp.Services.Lib.Controllers
{
    public class CacheController : ControllerBase
    {
        protected readonly ILogger<CacheController> _logger;
        protected readonly IDistributedCache _cache;

        public CacheController(ILogger<CacheController> logger, IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public async Task<TValue> CacheIt<TValue>(Func<Task<TValue>> queryTask, DateTime? timeout = null)
        {
            TValue result = default(TValue);
            string path = Request.Path;
            var value = await _cache.GetStringAsync(path);
            if (string.IsNullOrEmpty(value))
            {
                result = await queryTask.Invoke();
                await _cache.SetStringAsync(path, JsonConvert.SerializeObject(result), new DistributedCacheEntryOptions { AbsoluteExpiration = DateTime.Now.AddSeconds(60) });
            }
            else
            {
                result = JsonConvert.DeserializeObject<TValue>(value);
            }
            return result;
        }
    }
}
