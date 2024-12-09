using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Threading;

namespace ApiWithCache.Services
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, DateTimeOffset? absoluteExpiration = null);
        Task RemoveAsync(string key);
    }

    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _distributedCache.GetStringAsync(key);

            if (value is null)
                return default;

            return JsonSerializer.Deserialize<T>(value);
        }

        public async Task SetAsync<T>(string key, T value, DateTimeOffset? absoluteExpiration = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = absoluteExpiration ?? DateTimeOffset.Now.AddHours(1)
            };

            var serializedValue = JsonSerializer.Serialize(value);

            await _distributedCache.SetStringAsync(key, serializedValue, options);
        }

        public async Task RemoveAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }
    }
}
