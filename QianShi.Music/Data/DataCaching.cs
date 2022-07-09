using QianShi.Music.Common.Helpers;
using QianShi.Music.Models;

namespace QianShi.Music.Data
{
    public abstract class DataCaching<TResult, TParam> : IDataProvider<TResult, TParam>
        where TResult : IDataModel
        where TParam : IEquatable<TParam>
    {
        private readonly ConcurrentFixedSizeCache<TParam, TResult> _cache = new(10, 1);

        public async Task<TResult?> GetDataAsync(TParam param)
        {
            if (_cache.ContainsKey(param))
            {
                return _cache[param];
            }

            var result = await Source(param);

            if (result != null)
            {
                _cache[param] = result;
            }

            return result;
        }

        protected abstract Task<TResult?> Source(TParam param);
    }
}
