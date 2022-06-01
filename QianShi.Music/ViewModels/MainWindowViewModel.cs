using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using QianShi.Music.Common;
using QianShi.Music.Common.Models;
using QianShi.Music.Common.Models.Response;
using QianShi.Music.Extensions;
using QianShi.Music.Services;
using QianShi.Music.Views;

using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace QianShi.Music.ViewModels
{
    public class MainWindowViewModel : BindableBase, IConfigureService
    {
        private readonly IPlaylistService _playlistService;
        private readonly IPlayService _playService;
        private readonly IPlayStoreService _playStoreService;
        private readonly IRegionManager _regionManager;
        private Song? _currentSong = null;
        private bool _isMuted = false;
        private bool _isPlaying = false;
        private IRegionNavigationJournal _journal = null!;
        private ObservableCollection<MenuBar> _menuBars;
        private MenuBar? _navigateCurrentItem;
        private DelegateCommand _nextCommand = default!;
        private DelegateCommand _pauseCommand = default!;
        private DelegateCommand _playCommand = default!;
        private DelegateCommand _playingListSwitchCommand = default!;
        private DelegateCommand _previousCommand = default!;
        private DelegateCommand<bool?> _setMutedCommand = default!;
        private DelegateCommand<double?> _setVolumeCommand = default!;
        private double _songDuration = 1d;
        private double _songPosition = 0d;
        private UserData _userData;
        private double _volume = 0.5;

        public MainWindowViewModel(
            IRegionManager regionManager,
            IPlaylistService playlistService,
            IPlayService playService,
            IPlayStoreService playStoreService)
        {
            _regionManager = regionManager;
            _menuBars = new ObservableCollection<MenuBar>();
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            GoBackCommand = new DelegateCommand(() =>
            {
                if (_journal.CanGoBack)
                    _journal.GoBack();
            });
            GoForwardCommand = new DelegateCommand(() =>
            {
                if (_journal.CanGoForward)
                    _journal.GoForward();
            });
            LogoutCommand = new DelegateCommand(Logout);
            LoginCommand = new DelegateCommand(Login);
            OpenPlayViewCommand = new DelegateCommand<ContentControl>(OpenPlayView);
            SearchCommand = new DelegateCommand<string>(Search);
            _playlistService = playlistService;
            _userData = UserData.Instance;
            _playService = playService;
            _playStoreService = playStoreService;
            _playService.IsPlayingChanged += (s, e) =>
            {
                IsPlaying = e.NewValue;
            };
            _playService.ProgressChanged += (s, e) =>
            {
                SongPosition = e.Value;
                SongDuration = e.Total;
            };
            Volume = _playService.Volume;
            _playService.VolumeChanged += (s, e) => Volume = e.NewValue;
            _playService.IsMutedChanged += (s, e) => IsMuted = e.NewValue;
            _playStoreService.CurrentChanged += (s, e) => CurrentSong = e.NewSong;
        }

        public Song? CurrentSong
        {
            get => _currentSong;
            set => SetProperty(ref _currentSong, value);
        }

        public DelegateCommand GoBackCommand { get; private set; }

        public DelegateCommand GoForwardCommand { get; private set; }

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

        public DelegateCommand LoginCommand { get; private set; }

        public DelegateCommand LogoutCommand { get; private set; }

        public ObservableCollection<MenuBar> MenuBars
        {
            get => _menuBars;
            set { _menuBars = value; RaisePropertyChanged(); }
        }

        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }

        public MenuBar? NavigateCurrentItem
        {
            get => _navigateCurrentItem;
            set
            {
                if (_navigateCurrentItem != value && null != value)
                {
                    NavigateCommand.Execute(value);
                }
                SetProperty(ref _navigateCurrentItem, value);
            }
        }

        public DelegateCommand NextCommand =>
            _nextCommand ??= new DelegateCommand(_playStoreService.Next);

        public DelegateCommand<ContentControl> OpenPlayViewCommand { get; private set; }

        public DelegateCommand PauseCommand =>
            _pauseCommand ??= new DelegateCommand(_playStoreService.Pause);

        public DelegateCommand PlayCommand =>
            _playCommand ??= new DelegateCommand(_playStoreService.Play);

        public DelegateCommand PlayingListSwitchCommand =>
            _playingListSwitchCommand ??= new(() =>
            {
                var view = _regionManager.Regions[PrismManager.MainViewRegionName].ActiveViews?.FirstOrDefault();

                if (view is PlayingListView playingListView)
                {
                    _journal.GoBack();
                }
                else
                {
                    _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(nameof(PlayingListView));
                }
            });

        public DelegateCommand PreviousCommand =>
            _previousCommand ??= new DelegateCommand(_playStoreService.Previous);

        public DelegateCommand<string> SearchCommand { get; private set; }

        public DelegateCommand<bool?> SetMutedCommand =>
            _setMutedCommand ??= new(ExecuteSetMutedCommand);

        public DelegateCommand<double?> SetVolumeCommand =>
            _setVolumeCommand ??= new((value) =>
            {
                if (value.HasValue)
                {
                    _playService.SetVolume(value.Value);
                }
            });

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

        public UserData UserData
        {
            get => _userData;
            set => SetProperty(ref _userData, value);
        }

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
            CreateMenuBar();

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

            _regionManager.Regions[PrismManager.MainViewRegionName].NavigationService.Navigated += NavigationService_Navigated;
            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(MenuBars[0].NameSpace, back =>
            {
                _journal = back.Context.NavigationService.Journal;
            });

            _regionManager.Regions[PrismManager.FullScreenRegionName].RequestNavigate("PlayView");

            var songResponse = await _playlistService.SongDetail(493735159.ToString());

            if (songResponse.Code == 200)
            {
                await _playStoreService.PlayAsync(songResponse.Songs.First());
                _playStoreService.Pause();
            }
        }

        private void CreateMenuBar()
        {
            MenuBars.Add(new MenuBar() { Icon = "Home", Title = "首页", NameSpace = "IndexView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookOutline", Title = "发现", NameSpace = "FoundView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookPlus", Title = "音乐库", NameSpace = "LibraryView", Auth = true });
        }

        private void ExecuteSetMutedCommand(bool? parameter)
        {
            if (parameter.HasValue)
            {
                _playService.SetMute(parameter.Value);
            }
        }

        private void Login()
        {
            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(nameof(LoginView));
        }

        private async void Logout()
        {
            var response = await _playlistService.Logout();
            if (response.Code == 200)
            {
                UserData.Clear();
                UserData.Save();
            }
        }

        private void Navigate(MenuBar obj)
        {
            if (string.IsNullOrWhiteSpace(obj.NameSpace))
                return;
            var region = _regionManager.Regions[PrismManager.MainViewRegionName];
            region.RequestNavigate(obj.NameSpace);
        }

        private void NavigationService_Navigated(object? sender, RegionNavigationEventArgs e)
        {
            var viewName = e.Uri.OriginalString;
            var menuBar = _menuBars.FirstOrDefault(x => x.NameSpace == viewName);

            if (menuBar != null && menuBar != NavigateCurrentItem)
            {
                _navigateCurrentItem = menuBar;
            }
            else
            {
                _navigateCurrentItem = null;
            }
            RaisePropertyChanged(nameof(NavigateCurrentItem));
        }

        private void OpenPlayView(ContentControl obj)
        {
            if (obj.Content is PlayView playView)
            {
                ((PlayViewModel)playView.DataContext).Display = true;
            }
        }

        private void Search(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText)) return;

            var regions = _regionManager.Regions[PrismManager.MainViewRegionName];

            var uri = _journal.CurrentEntry.Uri;   // TODO 导航BUG 待修复
            regions.RequestNavigate(nameof(SearchView), new NavigationParameters { { SearchViewModel.SearchTextParameter, searchText } });
        }
    }
}