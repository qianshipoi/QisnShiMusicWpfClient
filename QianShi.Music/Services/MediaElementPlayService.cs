using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace QianShi.Music.Services
{
    public class MediaElementPlayService : IVideoPlayService
    {
        private readonly MediaElement _mediaElement;
        private readonly DispatcherTimer _timer;

        public event EventHandler<ProgressEventArgs>? ProgressChanged;
        public event EventHandler<PropertyChangedEventArgs<bool>>? IsPlayingChanged;
        public event EventHandler<PropertyChangedEventArgs<double>>? VolumeChanged;
        public event EventHandler<PropertyChangedEventArgs<bool>>? IsMutedChanged;
        public event EventHandler? PlayEnded;

        public FrameworkElement Control => _mediaElement;
        private double _position;
        public double Position
        {
            get => _position;
            set
            {
                if (_position.Equals(value)) return;
                _position = value;
                ProgressChanged?.Invoke(this, new ProgressEventArgs(value, Total));
            }
        }
        public double Total
        {
            get;
            set;
        }
        private bool _isPlaying;
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                if (_isPlaying == value) return;
                _isPlaying = value;
                IsPlayingChanged?.Invoke(this, new(value, !value));
            }
        }
        public bool IsMuted => _mediaElement.IsMuted;
        public double Volume => _mediaElement.Volume;

        private string _url = string.Empty;

        public string Url
        {
            get => _url;
            set
            {
                if(_url.Equals(value)) return;
                _url = value;
                OnUrlChanged();
            }
        }

        private void OnUrlChanged()
        {
            var cache = IsPlaying;
            if(cache) Pause();
            SetProgress(0);
            _mediaElement.Source = new(Url);
            if(cache) Play();
        }

        public MediaElementPlayService()
        {
            _mediaElement = new();
            _mediaElement.LoadedBehavior = MediaState.Manual;
            _mediaElement.MediaOpened += (s, e) =>
            {
                if (_mediaElement.NaturalDuration.HasTimeSpan)
                {
                    Total = _mediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
                    ProgressChanged?.Invoke(this, new ProgressEventArgs(Position, Total));
                }
            };
            _mediaElement.MediaEnded += (s, e) => PlayEnded?.Invoke(s, e); 

            _timer = new();
            _timer.Interval = TimeSpan.FromMilliseconds(200);
            _timer.Tick += (s, e) => Position = _mediaElement.Position.TotalMilliseconds;
        }

        public void Play(string url)
        {
            if (string.IsNullOrWhiteSpace(url) || Url == url)
            {
                Play();
                return;
            }

            Url = url;
            if (IsPlaying) Pause();
            _mediaElement.Source = new(Url);
            _mediaElement.Play();
            _timer.Start();
            IsPlaying = true;
        }

        public void Play()
        {
            if (IsPlaying || string.IsNullOrWhiteSpace(_url)) return;
            _mediaElement.Play();
            _timer.Start();
            IsPlaying = true;
        }

        public void Pause()
        {
            _mediaElement.Pause();
            _timer.Stop();
            IsPlaying = false;
        }

        public void SetVolume(double value)
        {
            if (value > 1) value = 1;
            if (value < 0) value = 0;
            if (!Volume.Equals(value))
            {
                var oldVal = Volume;
                _mediaElement.Volume = value;
                VolumeChanged?.Invoke(this, new(value, oldVal));
            }
        }

        public void SetProgress(double value)
        {
            if (!_mediaElement.NaturalDuration.HasTimeSpan) return;
            _timer.Stop();
            try
            {
                if (value < 0) value = 0;
                if (value > _mediaElement.NaturalDuration.TimeSpan.TotalMilliseconds)
                    value = _mediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
                _mediaElement.Position = TimeSpan.FromMilliseconds(value);
                Position = value;
            }
            finally
            {
                _timer.Start();
            }
        }

        public void SetMute(bool isMute)
        {
            if (isMute == _mediaElement.IsMuted) return;
            _mediaElement.IsMuted = isMute;
            IsMutedChanged?.Invoke(this, new(isMute, !isMute));
        }
    }
}
