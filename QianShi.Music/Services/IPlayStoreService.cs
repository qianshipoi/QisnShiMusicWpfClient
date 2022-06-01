using QianShi.Music.Common.Models.Response;

using System.Collections.ObjectModel;

namespace QianShi.Music.Services
{
    public interface IPlayStoreService
    {
        event EventHandler<SongChangedEventArgs>? CurrentChanged;

        ObservableCollection<Song> LaterPlaylist { get; }
        ObservableCollection<Song> JumpTheQueuePlaylist { get; }
        Song? Current { get; }
        bool HasNext { get; }
        bool HasPrev { get; }

        Task AddPlaylistAsync(long playlistId, IEnumerable<Song> songs);

        Task AddJumpToQueueSongAsync(Song song);

        void Next();

        void Previous();

        void Play();

        void Pause();

        Task PlayAsync(Song song);

        void RemoveSong(Song song);

        void Clear();
    }
}