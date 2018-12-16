using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetApp.PlayWeb.Gateway
{
    public interface Cacheable
    {
        string Key { get; set; }

        DateTime Expire { get; set; }
    }

    public class CacheStore<T> where T : Cacheable
    {
        private readonly ConcurrentDictionary<string, T> _cache;

        public CacheStore()
        {
            _cache = new ConcurrentDictionary<string, T>();
        }

        public void Add(string key, T value)
        {
            _cache.AddOrUpdate(key, value, (k, v) => v);
        }

        public T Get(string key)
        {
            if (_cache.TryGetValue(key, out T value))
            {
                if (value.Expire > DateTime.Now)
                {
                    return value;
                }
                //过期的就不要了
                _cache.TryRemove(key, out value);
            }
            return default(T);
        }
    }
}
