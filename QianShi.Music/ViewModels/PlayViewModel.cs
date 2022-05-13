using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

using QianShi.Music.Common.Models.Response;
using QianShi.Music.Extensions;
using QianShi.Music.Services;
using QianShi.Music.Views;

using System.Windows;
using System.Windows.Media.Animation;

namespace QianShi.Music.ViewModels
{
    public class PlayViewModel : NavigationViewModel
    {
        public static PlayViewModel PlayViewModelDesign => App.Current.Container.Resolve<PlayViewModel>();

        private readonly IContainerProvider _containerProvider;
        private readonly IRegionManager _regionManager;
        private readonly IPlaylistService _playlistService;
        private readonly IPlayService _playService;
        private readonly IPlayStoreService _playStoreService;
        private PlayView? _playView = null;
        private bool _display = false;

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
                        _playView.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        marginAnimation.From = new Thickness(0);
                        marginAnimation.To = new Thickness(0, window.ActualHeight, 0, 0);
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

        private DelegateCommand _closeCommand = default!;

        public DelegateCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new DelegateCommand(() => Display = false));

        private DelegateCommand _nextCommand = default!;

        public DelegateCommand NextCommand =>
            _nextCommand ?? (_nextCommand = new DelegateCommand(_playStoreService.Next));

        private DelegateCommand _previousCommand = default!;

        public DelegateCommand PreviousCommand =>
            _previousCommand ?? (_previousCommand = new DelegateCommand(_playStoreService.Previous));

        private double _duration = 1d;

        public double Duration
        {
            get { return _duration; }
            set { SetProperty(ref _duration, value); }
        }

        private double _position = 0d;

        public double Position
        {
            get { return _position; }
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

        private bool _isPlaying = false;

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set { SetProperty(ref _isPlaying, value); }
        }

        private DelegateCommand _playCommand = default!;

        public DelegateCommand PlayCommand =>
            _playCommand ?? (_playCommand = new DelegateCommand(_playStoreService.Play));

        private DelegateCommand _pauseCommand = default!;

        public DelegateCommand PauseCommand =>
            _pauseCommand ?? (_pauseCommand = new DelegateCommand(_playStoreService.Pause));

        private DelegateCommand<double?> _setPositionCommand = default!;

        public DelegateCommand<double?> SetPositionCommand =>
            _setPositionCommand ?? (_setPositionCommand = new DelegateCommand<double?>((value) =>
            {
                _settingUp = false;
                if (value.HasValue)
                {
                    _playService.SetProgress(value.Value);
                }
            }));

        private bool _settingUp = false;

        private DelegateCommand<double?> _startSetPositionCommand = default!;

        public DelegateCommand<double?> StartSetPositionCommand =>
            _startSetPositionCommand ?? (_startSetPositionCommand = new DelegateCommand<double?>(ExecuteStartSetPositionCommand));

        private void ExecuteStartSetPositionCommand(double? value)
        {
            _settingUp = true;
        }

        private string _lyricString = string.Empty;

        public string LyricString
        {
            get { return _lyricString; }
            set { SetProperty(ref _lyricString, value); }
        }

        private Song? _currentSong = default!;

        public Song? CurrentSong
        {
            get { return _currentSong; }
            set { SetProperty(ref _currentSong, value); }
        }

        public PlayViewModel(
            IContainerProvider provider,
            IRegionManager regionManager,
            IPlaylistService playlistService,
            IPlayService playService, IPlayStoreService playStoreService)
            : base(provider)
        {
            _containerProvider = provider;
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
                return lyricResponse.Lrc.Lyric;
            }
            return string.Empty;
        }
    }
}