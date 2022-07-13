namespace QianShi.Music.Data
{
    public interface IFoundDataProvider
    {
        IFoundPlaylist CreatePlaylist(string catName);
    }
}
