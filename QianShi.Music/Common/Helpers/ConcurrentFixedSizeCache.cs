using System.Collections.Concurrent;

namespace QianShi.Music.Common.Helpers
{
    /// <summary>
    /// 线程安全定长缓存字典
    /// </summary>
    public class ConcurrentFixedSizeCache<TKey, TValue> where TKey : IEquatable<TKey>
    {
        private readonly ConcurrentDictionary<TKey, TValue> _concurrentDictronary;

        private readonly List<TKey> _orderTimeKey;

        private readonly int _capacity;

        private readonly int _removeCount;

        private readonly object _locker = new object();

        /// <summary>
        /// 线程安全定长缓存字典
        /// </summary>
        /// <param name="capacity">缓存上限</param>
        /// <param name="removeCount">上线后移除的数量</param>
        public ConcurrentFixedSizeCache(int capacity, int removeCount)
        {
            _concurrentDictronary = new ConcurrentDictionary<TKey, TValue>();
            _orderTimeKey = new();
            _capacity = capacity;
            _removeCount = removeCount;
        }

        public TValue this[TKey key]
        {
            get
            {
                return _concurrentDictronary[key];
            }
            set
            {
                AddCacheByTime(key, value);
            }
        }

        private void AddCacheByTime(TKey key, TValue value)
        {
            lock (_locker)
            {
                if (_orderTimeKey.Contains(key))
                {
                    _orderTimeKey.Remove(key);
                }

                _concurrentDictronary[key] = value;

                _orderTimeKey.Add(key);

                if (_concurrentDictronary.Count <= _capacity) return;


                int removeIndex = 0;
                int hasRemoveCount = 0;

                while (hasRemoveCount < _removeCount && (removeIndex < _orderTimeKey.Count))
                {
                    var earliesKey = _orderTimeKey[removeIndex];

                    if (!_concurrentDictronary.ContainsKey(earliesKey))
                    {
                        removeIndex++;
                        continue;
                    }

                    var isRemoveSuccess = TryRemove(earliesKey, out var _);
                    if (isRemoveSuccess)
                    {
                        _orderTimeKey.RemoveAt(removeIndex);
                        hasRemoveCount++;
                    }
                    else
                    {
                        removeIndex++;
                    }
                }
            }
        }

        private bool TryRemove(TKey key, out TValue? value)
        {
            return _concurrentDictronary.TryRemove(key, out value);
        }

        public bool ContainsKey(TKey key)
        {
            return _concurrentDictronary.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue? val)
        {
            return _concurrentDictronary.TryGetValue(key, out val);
        }
    }
}
