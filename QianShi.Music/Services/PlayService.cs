using QianShi.Music.Common.Models.Response;

using System.Windows.Media;
using System.Windows.Threading;

namespace QianShi.Music.Services
{
    public class PlayService : IPlayService
    {
        private readonly MediaPlayer _mediaPlayer;
        private readonly IPlaylistService _playlistService;
        private readonly DispatcherTimer _timer;

        public event EventHandler<SongChangedEventArgs>? CurrentChanged;

        public event EventHandler<IsPlayingChangedEventArgs>? IsPlayingChanged;

        public event EventHandler<ProgressEventArgs>? ProgressChanged;

        public List<Song> ToPlay = new();

        public List<Song> JumpPlay = new();

        private List<Song> _playlist = new List<Song>(); // 播放列表

        public PlayService(IPlaylistService playlistService)
        {
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.MediaOpened += (s, e) => { };
            _mediaPlayer.MediaFailed += (s, e) => { };
            _mediaPlayer.MediaEnded += (s, e) => Next();

            _timer = new();
            _timer.Tick += (s, e) =>
                ProgressChanged?.Invoke(this, new ProgressEventArgs(_mediaPlayer.Position.TotalMilliseconds, _mediaPlayer.NaturalDuration.TimeSpan.TotalMilliseconds));

            _timer.Interval = TimeSpan.FromMilliseconds(500);

            _playlistService = playlistService;
        }

        private Song? _currentSong;

        public Song? Current
        {
            get => _currentSong;
            private set
            {
                if (_currentSong != null && _currentSong != value)
                {
                    _currentSong.IsPlaying = false;
                }
                if (_currentSong != value)
                    CurrentChanged.Invoke(this, new SongChangedEventArgs(value));
                _currentSong = value;
                if (_currentSong != null)
                    _currentSong.IsPlaying = true;
            }
        }

        private bool _isPlaying;
        public bool IsPlaying
        {
            get => _isPlaying;
            private set
            {
                if (value != _isPlaying)
                {
                    IsPlayingChanged.Invoke(this, new IsPlayingChangedEventArgs(value));
                }
                _isPlaying = value;
                if (_isPlaying)
                {
                    _timer.Start();
                }
                else
                {
                    _timer.Stop();
                }
            }
        }

        public void Add(Song song)
        {
            if (Current == null) Current = song;
            ToPlay.Add(song);
            _playlist.Add(song);
        }

        public void Add(IEnumerable<Song> songs)
        {
            if (Current == null) Current = songs.First();
            ToPlay.AddRange(songs);
            _playlist.AddRange(songs);
        }

        public void Clear()
        {
            ToPlay.Clear();
            JumpPlay.Clear();
            _playlist.Clear();
        }

        public void JumpToQueue(Song song)
        {
            if (Current == null) Current = song;
            JumpPlay.Add(song);
            _playlist.Insert(0, song);
        }

        public void Next()
        {
            if (_currentSong == null)
            {
                Current = _playlist.First();
                return;
            }

            var index = _playlist.IndexOf(_currentSong);
            if (index == -1 || index == ToPlay.Count - 1)
            {
                Current = _playlist.First();
                return;
            }
            else
            {
                Current = _playlist[index + 1];
            }
        }

        public void Pause()
        {
            _mediaPlayer.Pause();
            IsPlaying = false;
        }

        public async void Play()
        {
            if (IsPlaying) return;
            if (Current == null)
            {
                Next();
                if (Current == null)
                    return;
            }

            if (_mediaPlayer.HasAudio)
            {
                _mediaPlayer.Play();
                IsPlaying = true;
                return;
            }

            var response = await _playlistService.SongUrl(new Common.Models.Request.SongUrlRequest
            {
                Ids = Current.Id.ToString()
            });
            if (response.Code == 200)
            {
                _mediaPlayer.Open(new Uri(response.Data.First().Url));
                _mediaPlayer.Play();
                IsPlaying = true;
            }
        }

        public void Previous()
        {
            if (_currentSong == null)
            {
                Current = _playlist.Last();
                return;
            }

            var index = _playlist.IndexOf(_currentSong);
            if (index == -1 || index == ToPlay.Count - 1)
            {
                Current = _playlist.First();
                return;
            }
            else
            {
                Current = _playlist[index - 1];
            }
        }

        public void Remove(Song song)
        {
            JumpPlay.Remove(song);
            ToPlay.Remove(song);
            _playlist.Remove(song);
        }
    }
}