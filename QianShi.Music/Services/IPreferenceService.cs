namespace QianShi.Music.Services
{
    public interface IPreferenceService
    {
        string Get(string key, string defaultValue);
        int Get(string key, int defaultValue);
        double Get(string key, double defaultValue);
        float Get(string key, float defaultValue);
        DateTime Get(string key, DateTime defaultValue);
        long Get(string key, long defaultValue);
        bool Get(string key, bool defaultValue);

        void Set(string key, string value);
        void Set(string key, int value);
        void Set(string key, double value);
        void Set(string key, float value);
        void Set(string key, DateTime value);
        void Set(string key, bool value);
        void Set(string key, long value);

        bool ContainsKey(string key);
        void RemoveKey(string key);
        void Clear();

        void Save();
    }
}
