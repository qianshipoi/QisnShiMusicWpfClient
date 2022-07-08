using QianShi.Music.Models;

namespace QianShi.Music.Data
{
    public interface IDataProvider<TResult> where TResult : IDataModel
    {
        Task<TResult?> GetDataAsync();
    }

    public interface IDataProvider<TResult, TParam> where TResult : IDataModel
    {
        Task<TResult?> GetDataAsync(TParam param);
    }
}
