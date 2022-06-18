using QianShi.Music.Common.Models.Response;

namespace QianShi.Music.Services
{
    public interface IPlaylistStoreService
    {
        IReadOnlyList<Song> LikedSongs { get; }

        Task GetLikedSongsAsync();

        bool HasLikedSong(Song song);

        bool HasLikedSong(long id);
    }
}