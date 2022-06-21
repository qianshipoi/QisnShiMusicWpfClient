namespace QianShi.Music.Services
{
    public class JsonFilePreferenceService : IPreferenceService
    {
        static readonly object locker = new object();

        private const string FileName = "preference.json";
        private readonly string _baseDir = AppDomain.CurrentDomain.BaseDirectory;

        private Dictionary<string, object> _cache = default!;

        public JsonFilePreferenceService()
        {
            Initialization();
            Application.Current.Exit += Current_Exit;
        }

        private void Initialization()
        {
            var jsonPath = Path.Combine(_baseDir, FileName);

            if(!File.Exists(jsonPath))
            {
                _cache = new Dictionary<string, object>();
                return;
            }

            var jsonStr = File.ReadAllText(jsonPath);

            var cache = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonStr);
            _cache = cache ?? new Dictionary<string, object>();
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            var jsonPath = Path.Combine(_baseDir, FileName);
            var jsonStr = JsonSerializer.Serialize(_cache);
            File.WriteAllText(jsonPath, jsonStr);
        }

        public bool ContainsKey(string key)
        {
            lock (locker)
            {
                return _cache.ContainsKey(key);
            }
        }

        public string Get(string key, string defaultValue)
        {
            lock (locker)
            {
                if (_cache.ContainsKey(key))
                {
                    return _cache[key].ToString() ?? defaultValue;
                }
                else
                {
                    return defaultValue;
                }
            }
        }

        public int Get(string key, int defaultValue)
        {
            lock (locker)
            {
                if (_cache.ContainsKey(key))
                {
                    return int.TryParse(_cache[key].ToString(), out int result) ? result : defaultValue;
                }
                else
                {
                    return defaultValue;
                }
            }
        }

        public double Get(string key, double defaultValue)
        {
            lock (locker)
            {
                if (_cache.ContainsKey(key))
                {
                    return double.TryParse(_cache[key].ToString(), out double result) ? result : defaultValue;
                }
                else
                {
                    return defaultValue;
                }
            }
        }

        public float Get(string key, float defaultValue)
        {
            lock (locker)
            {
                if (_cache.ContainsKey(key))
                {
                    return float.TryParse(_cache[key].ToString(), out float result) ? result : defaultValue;
                }
                else
                {
                    return defaultValue;
                }
            }
        }

        public DateTime Get(string key, DateTime defaultValue)
        {
            lock (locker)
            {
                if (_cache.ContainsKey(key))
                {
                    return DateTime.TryParse(_cache[key].ToString(), out DateTime result) ? result : defaultValue;
                }
                else
                {
                    return defaultValue;
                }
            }
        }

        public long Get(string key, long defaultValue)
        {
            lock (locker)
            {
                if (_cache.ContainsKey(key))
                {
                    return long.TryParse(_cache[key].ToString(), out long result) ? result : defaultValue;
                }
                else
                {
                    return defaultValue;
                }
            }
        }

        private void InternalSet(string key, object value)
        {
            lock (locker)
            {
                _cache[key] = value;
            }
        }

        public void Set(string key, string value) => InternalSet(key, value);

        public void Set(string key, int value) => InternalSet(key, value);

        public void Set(string key, double value) => InternalSet(key, value);

        public void Set(string key, float value) => InternalSet(key, value);

        public void Set(string key, DateTime value) => InternalSet(key, value);

        public void Set(string key, bool value) => InternalSet(key, value);

        public void Set(string key, long value) => InternalSet(key, value);

        public void RemoveKey(string key)
        {
            lock (locker)
            {
                _cache.Remove(key);
            }
        }

        public void Clear()
        {
            lock (locker)
            {
                _cache.Clear();
            }
        }
    }
}
