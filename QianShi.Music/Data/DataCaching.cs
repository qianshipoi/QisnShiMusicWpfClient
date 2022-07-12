using Microsoft.Extensions.Caching.Memory;

using QianShi.Music.Common.Helpers;
using QianShi.Music.Models;

namespace QianShi.Music.Data
{
    public abstract class DataCaching<TResult, TParam> : IDataProvider<TResult, TParam>
        where TResult : IDataModel
        where TParam : IEquatable<TParam>
    {
        private readonly ConcurrentFixedSizeCache<TParam, TResult> _cache = new(10, 1);

        private readonly IMemoryCache _memoryCache;

        public DataCaching(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<TResult?> GetDataAsync(TParam param)
        {
            if (_cache.ContainsKey(param))
            {
                //return _memoryCache.Get<TResult?>(param);
                return _cache[param];
            }

            var result = await Source(param);

            if (result != null)
            {
                //_memoryCache.Set(param, result,TimeSpan.FromMinutes(30));
                _cache[param] = result;
            }

            return result;
        }

        protected abstract Task<TResult?> Source(TParam param);
    }
}
