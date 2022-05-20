using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

using Prism.Ioc;

using QianShi.Music.Views;

namespace QianShi.Music.Services
{
    public class MediaElementPlayService : IVideoPlayService
    {
        private readonly IContainerProvider _containerProvider;
        private readonly MediaElement _mediaElement;
        private readonly DispatcherTimer _timer;
        private string _url = string.Empty;
        private bool _isPlaying;
        private double _position;
        private string? _cover;
        private bool _isFullScreen = false;
        private VideoPlayWindow? _window;

        public event EventHandler<ProgressEventArgs>? ProgressChanged;
        public event EventHandler<PropertyChangedEventArgs<bool>>? IsPlayingChanged;
        public event EventHandler<PropertyChangedEventArgs<double>>? VolumeChanged;
        public event EventHandler<PropertyChangedEventArgs<bool>>? IsMutedChanged;
        public event EventHandler<PropertyChangedEventArgs<string>>? CoverChanged;
        public event EventHandler<PropertyChangedEventArgs<bool>>? IsFullScreenChanged;
        public event EventHandler? PlayEnded;

        public FrameworkElement Control => _mediaElement;
        public string? Cover
        {
            get => _cover;
            set
            {
                if (_cover == value) return;
                var oldVal = _cover;
                _cover = value;
                CoverChanged?.Invoke(this, new(_cover, oldVal));
            }
        }
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
        public string Url
        {
            get => _url;
            set
            {
                if (_url.Equals(value)) return;
                _url = value;
                OnUrlChanged();
            }
        }
        public bool IsFullScreen
        {
            get => _isFullScreen;
            set
            {
                if (_isFullScreen == value) return;
                _isFullScreen = value;
                IsFullScreenChanged?.Invoke(this, new(_isFullScreen, !IsFullScreen));
            }
        }

        private void OnUrlChanged()
        {
            var cache = IsPlaying;
            if (cache) Pause();
            SetProgress(0);
            _mediaElement.Source = new(Url);
            if (cache) Play();
        }

        public MediaElementPlayService(IContainerProvider containerProvider)
        {
            _containerProvider = containerProvider;
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
        public void FullScreen()
        {
            if (IsFullScreen)
            {
                IsFullScreen = false;
                _window!.Close();
                _window = null;
            }
            else
            {
                _window = _containerProvider.Resolve<VideoPlayWindow>();
                _window.Show();
                IsFullScreen = true;
            }
        }
    }
}
