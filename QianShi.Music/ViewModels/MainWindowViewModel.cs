using QianShi.Music.Common;
using QianShi.Music.Common.Models.Response;
using QianShi.Music.Extensions;
using QianShi.Music.Services;
using QianShi.Music.Views;
using QianShi.Music.Views.Navigation;

namespace QianShi.Music.ViewModels
{
    public class MainWindowViewModel : BindableBase, IConfigureService
    {
        private readonly ISnackbarMessageQueue _snackbarMessageQueue;
        private readonly INavigationService _navigationService;
        private readonly IPlaylistService _playlistService;
        private readonly IPlayService _playService;
        private readonly IPlayStoreService _playStoreService;
        private readonly IRegionManager _regionManager;
        private Song? _currentSong = null;
        private bool _isMuted = false;
        private bool _isPlaying = false;
        private DelegateCommand _nextCommand = default!;
        private DelegateCommand<ContentControl> _openPlayViewCommand = default!;
        private DelegateCommand _pauseCommand = default!;
        private DelegateCommand _playCommand = default!;
        private DelegateCommand _playingListSwitchCommand = default!;
        private DelegateCommand _previousCommand = default!;
        private DelegateCommand<bool?> _setMutedCommand = default!;
        private DelegateCommand _settingCommand = default!;
        private DelegateCommand<double?> _setVolumeCommand = default!;
        private double _songDuration = 1d;
        private double _songPosition = 0d;
        private double _volume = 0.5;
        public SnackbarMessageQueue SnackbarMessageQueue => (SnackbarMessageQueue)_snackbarMessageQueue;

        public MainWindowViewModel(
            IRegionManager regionManager,
            IPlaylistService playlistService,
            IPlayService playService,
            IPlayStoreService playStoreService,
            INavigationService navigationService,
            ISnackbarMessageQueue snackbarMessageQueue)
        {
            _regionManager = regionManager;
            _playlistService = playlistService;
            _playService = playService;
            _playStoreService = playStoreService;
            _navigationService = navigationService;
            _playService.IsPlayingChanged += (s, e) => IsPlaying = e.NewValue;
            _playService.ProgressChanged += (s, e) =>
            {
                SongPosition = e.Value;
                SongDuration = e.Total;
            };
            Volume = _playService.Volume;
            _playService.VolumeChanged += (s, e) => Volume = e.NewValue;
            _playService.IsMutedChanged += (s, e) => IsMuted = e.NewValue;
            _playStoreService.CurrentChanged += (s, e) => CurrentSong = e.NewSong;
            _snackbarMessageQueue = snackbarMessageQueue;
        }


        public Song? CurrentSong
        {
            get => _currentSong;
            set => SetProperty(ref _currentSong, value);
        }
        public bool IsMuted
        {
            get => _isMuted;
            set => SetProperty(ref _isMuted, value);
        }

        public bool IsPlaying
        {
            get => _isPlaying;
            set => SetProperty(ref _isPlaying, value);
        }
        public DelegateCommand NextCommand =>
            _nextCommand ??= new(_playStoreService.Next);

        public DelegateCommand<ContentControl> OpenPlayViewCommand =>
            _openPlayViewCommand ?? (_openPlayViewCommand = new(OpenPlayView));

        public DelegateCommand PauseCommand =>
            _pauseCommand ??= new(_playStoreService.Pause);

        public DelegateCommand PlayCommand =>
            _playCommand ??= new(_playStoreService.Play);

        public DelegateCommand PlayingListSwitchCommand =>
            _playingListSwitchCommand ??= new(() =>
            {
                var view = _regionManager.Regions[PrismManager.MainViewRegionName].ActiveViews?.FirstOrDefault();

                if (view is PlayingListView playingListView)
                {
                    if (_navigationService.CanGoBack)
                        _navigationService.GoBack();
                }
                else
                {
                    _navigationService.NavigateToPlayingList();
                }
            });

        public DelegateCommand PreviousCommand =>
            _previousCommand ??= new(_playStoreService.Previous);

        public DelegateCommand<bool?> SetMutedCommand =>
            _setMutedCommand ??= new(ExecuteSetMutedCommand);

        public DelegateCommand SettingCommand =>
            _settingCommand ??= new(() => _navigationService.MainRegionNavigation(nameof(SettingView)));

        public DelegateCommand<double?> SetVolumeCommand =>
            _setVolumeCommand ??= new((value) => _playService.SetVolume(value ?? _playService.Volume));

        public double SongDuration
        {
            get => _songDuration;
            set => SetProperty(ref _songDuration, value);
        }

        public double SongPosition
        {
            get => _songPosition;
            set => SetProperty(ref _songPosition, value);
        }

        public UserData UserData => UserData.Instance;

        public double Volume
        {
            get => _volume;
            set => SetProperty(ref _volume, value);
        }

        /// <summary>
        /// 配置首页初始化参数
        /// </summary>
        public async void Configure()
        {
            var response = await _playlistService.LoginStatus();
            if (response.Data.Code == 200)
            {
                if (response.Data.Account == null || response.Data.Profile == null)
                {
                    UserData.Clear();
                }
                else
                {
                    UserData.Cover = response.Data.Profile?.AvatarUrl;
                    UserData.NickName = response.Data.Profile?.Nickname;
                    UserData.Id = response.Data.Account?.Id ?? 0;
                    UserData.VipType = response.Data.Account?.VipType ?? 0;
                }
                UserData.Save();
            }
            _regionManager.Regions[PrismManager.NavigateBarRegionName].RequestNavigate(nameof(NavigationBarView));

            _regionManager.Regions[PrismManager.FullScreenRegionName].RequestNavigate("PlayView");

            var songResponse = await _playlistService.SongDetail(493735159.ToString());

            if (songResponse.Code == 200)
            {
                await _playStoreService.PlayAsync(songResponse.Songs.First());
                _playStoreService.Pause();
            }
        }

        private void ExecuteSetMutedCommand(bool? parameter)
        {
            if (parameter.HasValue)
            {
                _playService.SetMute(parameter.Value);
            }
        }

        private void OpenPlayView(ContentControl obj)
        {
            if (obj.Content is PlayView playView)
            {
                ((PlayViewModel)playView.DataContext).Display = true;
            }
        }
    }
}