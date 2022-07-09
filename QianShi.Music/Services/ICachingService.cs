namespace QianShi.Music.Services
{
    public interface ICachingService
    {
        Task SetAsync<T>(string key, T value);

        Task<T?> GetAsync<T>(string key);
    }
}
