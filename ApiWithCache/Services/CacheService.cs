using Microsoft.Extensions.Caching.Memory;

namespace ApiWithCache.Services
{
    public interface ICacheService
    {
        T Get<T>(string key);
        void Set<T>(string key, T value);
    }

    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T Get<T>(string key)
        {
            var value = _memoryCache.Get<T>(key);

            return value;
        }

        public void Set<T>(string key, T value)
        {
            _memoryCache.Set(key, value);
        }
    }
}
