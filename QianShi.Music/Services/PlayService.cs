using QianShi.Music.Common.Models.Response;

using System.Collections.ObjectModel;
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

        public event EventHandler<VolumeChangedEventArgs>? VolumeChanged;

        public event EventHandler<PropertyChangedEventArgs<bool>>? IsMutedChanged;

        public ObservableCollection<Song> ToPlay { get; private set; } = new();

        public ObservableCollection<Song> JumpPlay { get; private set; } = new();

        private List<Song> _playlist = new(); // 播放列表

        public PlayService(IPlaylistService playlistService)
        {
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.MediaOpened += (s, e) =>
            { };
            _mediaPlayer.MediaFailed += (s, e) =>
            { };
            _mediaPlayer.MediaEnded += (s, e) => Next();

            _timer = new();
            _timer.Tick += (s, e) =>
            {
                if (_mediaPlayer.NaturalDuration.HasTimeSpan)
                {
                    ProgressChanged?.Invoke(this, new ProgressEventArgs(_mediaPlayer.Position.TotalMilliseconds, _mediaPlayer.NaturalDuration.TimeSpan.TotalMilliseconds));
                }
            };

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
                {
                    OnCurrentChanged(_currentSong, value);
                    CurrentChanged?.Invoke(this, new SongChangedEventArgs(value));
                }
                _currentSong = value;
                if (_currentSong != null)
                    _currentSong.IsPlaying = true;
            }
        }

        private void OnCurrentChanged(Song? oldValue, Song? newValue)
        {
            if (null != oldValue)
            {
                ToPlay.Add(oldValue);
            }

            if (null != newValue)
            {
                var jumpPlaySong = JumpPlay.Where(x => x.Equals(newValue)).FirstOrDefault();
                if (null != jumpPlaySong)
                {
                    JumpPlay.Remove(jumpPlaySong);
                    return;
                }
                var toPlaySong = ToPlay.Where(x => x.Equals(newValue)).FirstOrDefault();
                if (null != toPlaySong)
                {
                    ToPlay.Remove(toPlaySong);
                }
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
                    IsPlayingChanged?.Invoke(this, new IsPlayingChangedEventArgs(value));
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

        public double Volume => _mediaPlayer.Volume;

        public bool IsMuted => _mediaPlayer.IsMuted;

        public void SetMute(bool isMute)
        {
            if (isMute == _mediaPlayer.IsMuted) return;
            _mediaPlayer.IsMuted = isMute;
            IsMutedChanged?.Invoke(this, new PropertyChangedEventArgs<bool>(isMute, !isMute));
        }

        public void SetVolume(double volume)
        {
            if (volume > 1) volume = 1;
            else if (volume < 0) volume = 0;
            _mediaPlayer.Volume = volume;
            VolumeChanged?.Invoke(this, new VolumeChangedEventArgs(volume));
        }

        public void SetProgress(double value)
        {
            if (Current == null) return;
            _timer.Stop();
            try
            {
                if (value < 0) value = 0;
                if (value > _mediaPlayer.NaturalDuration.TimeSpan.TotalMilliseconds)
                    value = _mediaPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
                _mediaPlayer.Position = TimeSpan.FromMilliseconds(value);
                ProgressChanged?.Invoke(this, new ProgressEventArgs(_mediaPlayer.Position.TotalMilliseconds, _mediaPlayer.NaturalDuration.TimeSpan.TotalMilliseconds));
            }
            finally
            {
                _timer.Start();
            }
        }

        public async void Add(Song song)
        {
            var response = await _playlistService.SongDetail(song.Id.ToString());

            if (response.Code == 200)
            {
                song = response.Songs.First();
                ToPlay.Add(song);
                _playlist.Add(song);
                if (Current == null) Current = song;
            }
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
                SetProgress(0);
                Play();
                return;
            }

            var index = _playlist.IndexOf(_currentSong);
            if (index == -1 || index == _playlist.Count - 1)
            {
                Current = _playlist.First();
            }
            else
            {
                Current = _playlist[index + 1];
            }
            SetProgress(0);
            Play();
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
                SetProgress(0);
                Play();
                return;
            }

            var index = _playlist.IndexOf(_currentSong);
            if (index == -1 || index == _playlist.Count - 1)
            {
                Current = _playlist.First();
            }
            else
            {
                Current = _playlist[index - 1];
            }
            SetProgress(0);
            Play();
        }

        public void Remove(Song song)
        {
            JumpPlay.Remove(song);
            ToPlay.Remove(song);
            _playlist.Remove(song);
        }
    }
}