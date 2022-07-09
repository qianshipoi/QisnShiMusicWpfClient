using System.Collections.Concurrent;

namespace QianShi.Music.Services
{
    public class MemoryCachingService : ICachingService
    {
        private readonly ConcurrentDictionary<string, object?> _cache = new();

        public Task<T?> GetAsync<T>(string key)
        {
            if (_cache.ContainsKey(key))
            {
                return Task.FromResult((T?)_cache[key]);
            }
            return Task.FromResult(default(T?));
        }

        public Task SetAsync<T>(string key, T value)
        {
            _cache.AddOrUpdate(key, value, (_, _) => value);

            return Task.CompletedTask;
        }
    }
}
