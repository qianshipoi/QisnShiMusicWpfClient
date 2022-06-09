using System.Windows.Media;
using System.Windows.Threading;

namespace QianShi.Music.Services
{
    public class MediaPlayerPlayService : IPlayService
    {
        private readonly MediaPlayer _mediaPlayer;
        private readonly DispatcherTimer _timer;
        private string _mediaUrl = string.Empty;
        private bool _isPlaying = false;

        public bool IsPlaying
        {
            get => _isPlaying;
            private set
            {
                if (value == _isPlaying) return;
                IsPlayingChanged?.Invoke(this, new PropertyChangedEventArgs<bool>(value, _isPlaying));
                _isPlaying = value;
            }
        }

        public bool IsMuted => _mediaPlayer.IsMuted;
        public double Volume => _mediaPlayer.Volume;

        public event EventHandler<ProgressEventArgs>? ProgressChanged;

        public event EventHandler<PropertyChangedEventArgs<bool>>? IsMutedChanged;

        public event EventHandler<PropertyChangedEventArgs<double>>? VolumeChanged;

        public event EventHandler<PropertyChangedEventArgs<bool>>? IsPlayingChanged;

        public event EventHandler? PlayEnded;

        public MediaPlayerPlayService()
        {
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.MediaOpened += (s, e) => { SendProgressChangEvent(null, EventArgs.Empty); };
            _mediaPlayer.MediaFailed += (s, e) => { };
            _mediaPlayer.MediaEnded += (s, e) => { PlayEnded?.Invoke(s, e); };
            _timer = new();
            _timer.Tick += SendProgressChangEvent;
            _timer.Interval = TimeSpan.FromMilliseconds(500);
        }

        private void SendProgressChangEvent(object? sender, EventArgs e)
        {
            if (_mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                ProgressChanged?.Invoke(this, new ProgressEventArgs(_mediaPlayer.Position.TotalMilliseconds, _mediaPlayer.NaturalDuration.TimeSpan.TotalMilliseconds));
            }
        }

        public void Play()
        {
            if (IsPlaying || string.IsNullOrWhiteSpace(_mediaUrl)) return;
            _mediaPlayer.Play();
            _timer.Start();
            IsPlaying = true;
        }

        public void Play(string url)
        {
            if (url == _mediaUrl)
            {
                Play();
                return;
            }
            _mediaUrl = url;

            Pause();
            _mediaPlayer.Open(new Uri(url));
            _mediaPlayer.Position = TimeSpan.Zero;
            Play();
        }

        public void Pause()
        {
            if (!IsPlaying) return;
            _mediaPlayer.Pause();
            _timer.Stop();
            IsPlaying = false;
        }

        public void SetMute(bool isMute)
        {
            if (isMute == _mediaPlayer.IsMuted) return;
            _mediaPlayer.IsMuted = isMute;
            IsMutedChanged?.Invoke(this, new PropertyChangedEventArgs<bool>(isMute, !isMute));
        }

        public void SetVolume(double volume)
        {
            if (volume == Volume) return;
            if (volume > 1) volume = 1;
            else if (volume < 0) volume = 0;
            var oldVal = _mediaPlayer.Volume;
            _mediaPlayer.Volume = volume;
            VolumeChanged?.Invoke(this, new(volume, oldVal));
        }

        public void SetProgress(double value)
        {
            if (!_mediaPlayer.NaturalDuration.HasTimeSpan) return;
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
    }
}