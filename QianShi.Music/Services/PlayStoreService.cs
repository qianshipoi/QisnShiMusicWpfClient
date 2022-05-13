using QianShi.Music.Common.Models.Response;

using System.Collections.ObjectModel;

namespace QianShi.Music.Services
{
    public class PlayStoreService : IPlayStoreService
    {
        private readonly MediaPlayerPlayService _playService;
        private readonly IPlaylistService _playlistService;
        private readonly List<Song> _source = new();
        private readonly List<Song> _playlist = new();
        private long _playlistId;
        public ObservableCollection<Song> LaterPlaylist { get; } = new();
        public ObservableCollection<Song> JumpTheQueuePlaylist { get; } = new();

        public event EventHandler<SongChangedEventArgs>? CurrentChanged;

        private Song? _current;
        public Song? Current
        {
            get => _current;
            private set
            {
                if (value != _current)
                {
                    CurrentChanged?.Invoke(this, new(value));
                    _currentIndex = value == null ? -1 : _playlist.IndexOf(value);
                    _current = value;
                }
            }
        }

        private int _currentIndex = -1;

        public bool HasNext => _playlist.Count > 0 && _currentIndex < _playlist.Count - 1;

        public bool HasPrev => _playlist.Count > 0 && _currentIndex > 0;

        public PlayStoreService(IPlaylistService playlistService)
        {
            _playService = new MediaPlayerPlayService();
            _playService.PlayEnded += (s, e) =>
            {
                _playService.SetProgress(0);
                Next();
            };
            _playlistService = playlistService;
        }

        private async Task<IEnumerable<Song>?> GetSongUrl(IEnumerable<Song> songs)
        {
            var response = await _playlistService.SongUrl(new Common.Models.Request.SongUrlRequest
            {
                Ids = string.Join(',', songs.Select(x => x.Id))
            });

            if (response.Code == 200)
            {
                var urls = response.Data.ToDictionary(x => x.Id, x => x.Url);
                var songList = new List<Song>(urls.Count);

                songs.ToList().ForEach(song =>
                {
                    if (urls.TryGetValue(song.Id, out var url))
                    {
                        song.Url = url;
                        songList.Add(song);
                    }
                });
            }
            return null;
        }

        private async Task<Song?> GetSongUrl(Song song)
        {
            var result = await GetSongUrl(new List<Song> { song });
            if (result == null || result.Count() == 0) return null;
            return result.First();
        }

        public async Task AddPlaylistAsync(long playlistId, IEnumerable<Song> songs)
        {
            if (_playlistId == playlistId)
            {
                return;
            }

            _playlistId = playlistId;
            var newSongs = await GetSongUrl(songs);
            if (newSongs == null || newSongs.Count() == 0) return;

            Clear();
            _source.AddRange(songs);
            _playlist.AddRange(songs);
            LaterPlaylist.AddRange(songs);
        }

        public async Task AddJumpToQueueSongAsync(Song song)
        {
            var newSong = await GetSongUrl(song);
            if (newSong == null) return;

            if (Current == null)
            {
                _playlist.Insert(0, newSong);
            }
            else
            {
                var currentIndex = _playlist.FindIndex(song => song.Equals(Current));
                _playlist.Insert(currentIndex + 1, newSong);
                var currentIndexToSource = _source.FindIndex(song => song.Equals(Current));
                _source.Insert(currentIndexToSource + 1, newSong);
            }

            JumpTheQueuePlaylist.Add(newSong);
        }

        public void Next()
        {
            if (_playlist.Count == 0) return;

            if (_currentIndex == -1)
            {
                Current = _playlist.First();
            }
            else
            {
                if (_currentIndex < _playlist.Count - 1)
                {
                    Current = _playlist[_currentIndex + 1];
                }
                else
                {
                    Current = _playlist.First();
                }
            }
            Play();
        }

        public void Previous()
        {
            if (_playlist.Count == 0) return;

            if (_currentIndex == -1 || _currentIndex == 0)
            {
                Current = _playlist.Last();
            }
            else
            {
                Current = _playlist[_currentIndex - 1];
            }
            Play();
        }

        public void Play()
        {
            if (Current == null || _playlist.Count == 0) return;
            _playService.Play(Current.Url!);
        }

        public async Task PlayAsync(Song song)
        {
            var index = _playlist.FindIndex(s => s.Id == song.Id);
            if (index == -1)
            {
                var newSong = await GetSongUrl(song);
                if (newSong == null) return;
                _playlist.Insert(_currentIndex + 1, newSong);
                Current = newSong;
            }
            else
            {
                Current = _playlist[index];
            }

            Play();
        }

        public void Pause() => _playService.Pause();

        public void RemoveSong(Song song)
        {
            _source.Remove(song);
            _playlist.Remove(song);
            LaterPlaylist.Remove(song);
            JumpTheQueuePlaylist.Remove(song);
        }

        public void Clear()
        {
            _source.Clear();
            _playlist.Clear();
            LaterPlaylist.Clear();
            JumpTheQueuePlaylist.Clear();
        }
    }
}
