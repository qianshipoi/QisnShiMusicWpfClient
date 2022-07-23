using QianShi.Music.Common.Helpers;
using QianShi.Music.Common.Models.Response;
using QianShi.Music.Extensions;
using QianShi.Music.Services;
using QianShi.Music.Views;

using System.Windows.Media.Animation;

namespace QianShi.Music.ViewModels
{
    public class PlayViewModel : NavigationViewModel
    {
        private readonly IPlaylistService _playlistService;
        private readonly IPlayService _playService;
        private readonly IPlayStoreService _playStoreService;
        private readonly IRegionManager _regionManager;
        private DelegateCommand _closeCommand = default!;
        private Song? _currentSong = default!;
        private bool _display = false;
        private double _duration = 1d;
        private bool _isPlaying = false;
        private string _lyricString = string.Empty;
        private DelegateCommand _nextCommand = default!;
        private DelegateCommand _pauseCommand = default!;
        private DelegateCommand _playCommand = default!;
        private PlayView? _playView = null;
        private double _position = 0d;
        private DelegateCommand _previousCommand = default!;
        private DelegateCommand<double?> _setPositionCommand = default!;
        private bool _settingUp = false;
        private DelegateCommand<double?> _startSetPositionCommand = default!;
        private DelegateCommand _fullScreenCommand = default!;

        public PlayViewModel(
            IContainerProvider provider,
            IRegionManager regionManager,
            IPlaylistService playlistService,
            IPlayService playService,
            IPlayStoreService playStoreService)
            : base(provider)
        {
            _regionManager = regionManager;
            _playlistService = playlistService;
            _playService = playService;
            _playStoreService = playStoreService;
            _playService.ProgressChanged += (s, e) =>
            {
                Position = e.Value;
                Duration = e.Total;
            };
            _playService.IsPlayingChanged += (s, e) => IsPlaying = e.NewValue;
            _playStoreService.CurrentChanged += async (s, e) =>
            {
                if (e.NewSong != null)
                {
                    LyricString = await GetLyric(e.NewSong.Id);
                }
                CurrentSong = e.NewSong;
            };
        }


        public DelegateCommand FullScreenCommand =>
            _fullScreenCommand ??= new DelegateCommand(() =>
            {
                var mainWindow = App.Current.MainWindow;
                FullScreenHelper.StartFullScreen(mainWindow);
            });

        public DelegateCommand CloseCommand =>
            _closeCommand ??= new(() => Display = false);

        public Song? CurrentSong
        {
            get => _currentSong;
            set => SetProperty(ref _currentSong, value);
        }

        public bool Display
        {
            get => _display;
            set
            {
                if (_playView != null && _display != value)
                {
                    var marginAnimation = new ThicknessAnimation();
                    var window = Window.GetWindow(_playView);
                    if (value)
                    {
                        SetProperty(ref _display, value);
                        marginAnimation.From = new Thickness(0, window.ActualHeight, 0, 0);
                        marginAnimation.To = new Thickness(0);
                        marginAnimation.EasingFunction = new CubicEase()
                        {
                            EasingMode = EasingMode.EaseIn,
                        };
                        _playView.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        marginAnimation.From = new Thickness(0);
                        marginAnimation.To = new Thickness(0, window.ActualHeight, 0, 0);
                        marginAnimation.EasingFunction = new CubicEase()
                        {
                            EasingMode = EasingMode.EaseOut,
                        };
                        marginAnimation.Completed += (s, e) =>
                        {
                            SetProperty(ref _display, value);
                            _playView.Visibility = Visibility.Collapsed;
                        };
                    }

                    marginAnimation.Duration = TimeSpan.FromSeconds(0.5);

                    _playView.BeginAnimation(FrameworkElement.MarginProperty, marginAnimation);
                }
                else
                {
                    SetProperty(ref _display, value);
                }
            }
        }

        public double Duration
        {
            get => _duration;
            set => SetProperty(ref _duration, value);
        }

        public bool IsPlaying
        {
            get => _isPlaying;
            set => SetProperty(ref _isPlaying, value);
        }

        public string LyricString
        {
            get => _lyricString;
            set => SetProperty(ref _lyricString, value);
        }

        public DelegateCommand NextCommand =>
            _nextCommand ??= new(_playStoreService.Next);

        public DelegateCommand PauseCommand =>
            _pauseCommand ??= new(_playStoreService.Pause);

        public DelegateCommand PlayCommand =>
            _playCommand ??= new(_playStoreService.Play);


        public double Position
        {
            get => _position;
            set
            {
                if (!_settingUp)
                    SetProperty(ref _position, value);
                else
                {
                    _position = value;
                }
            }
        }

        public DelegateCommand PreviousCommand =>
            _previousCommand ??= new(_playStoreService.Previous);

        public DelegateCommand<double?> SetPositionCommand =>
            _setPositionCommand ??= new((value) =>
            {
                _settingUp = false;
                if (value.HasValue)
                {
                    _playService.SetProgress(value.Value);
                }
            });

        public DelegateCommand<double?> StartSetPositionCommand =>
            _startSetPositionCommand ??= new(_ => _settingUp = true);

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            var region = _regionManager.Regions[PrismManager.FullScreenRegionName];
            if (region != null)
            {
                var view = region.Views.FirstOrDefault(x => x.GetType() == typeof(PlayView));
                if (view != null)
                {
                    _playView = view as PlayView;
                }
            }
        }

        private async Task<string> GetLyric(long songId)
        {
            var lyricResponse = await _playlistService.Lyric(songId);

            if (lyricResponse.Code == 200)
            {
                return lyricResponse.Lrc?.Lyric ?? String.Empty;
            }
            return string.Empty;
        }
    }
}