using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

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

        private static readonly ConcurrentDictionary<string, string> memoryCache = new ConcurrentDictionary<string, string>();
        private static readonly ConcurrentDictionary<string, object> memoryCacheRaw = new ConcurrentDictionary<string, object>();
        private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(100);
        private static volatile int threadCount = 0;

        public async Task<TValue> CacheIt<TValue>(Func<Task<TValue>> queryTask, DateTime? timeout = null) where TValue : new()
        {
            //semaphoreSlim.Wait();
            //semaphoreSlim.Release();
            int threadCnt = threadCount;
            Interlocked.Increment(ref threadCount);
            Console.WriteLine($"{threadCnt} at {Thread.CurrentThread.ManagedThreadId}");

            TValue result;
            string key = $"{Request.Path}{Request.QueryString}";
            bool fromCache = false;
            // 1.字符串太大，会导致性能下降?
            // 2.redis没有更快
            var raw = await _cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(raw))
            {
                fromCache = true;
            }
            else
            {
                var inDb = await queryTask.Invoke();
                raw = JsonConvert.SerializeObject(inDb);
                await _cache.SetStringAsync(key, raw);
            }
            result = JsonConvert.DeserializeObject<TValue>(raw);
            Console.WriteLine($"{threadCnt} complete {fromCache} length:{raw.Length}");
            /*
            if (memoryCache.TryGetValue(key, out var raw))
            {
                fromCache = true;
            }
            else
            {
                var inDb = await queryTask.Invoke();
                raw = JsonConvert.SerializeObject(inDb);
                memoryCache.TryAdd(key, raw);
            }
            result = JsonConvert.DeserializeObject<TValue>(raw);
            Console.WriteLine($"{threadCnt} complete {fromCache} length:{raw.Length}");
            if (memoryCacheRaw.TryGetValue(key, out var raw))
            {
                fromCache = true;
            }
            else
            {
                raw = await queryTask.Invoke();
                memoryCacheRaw.TryAdd(key, raw);
            }
            result = (TValue)raw;
            Console.WriteLine($"{threadCnt} complete {fromCache}");*/
            return result;
        }
    }
}
