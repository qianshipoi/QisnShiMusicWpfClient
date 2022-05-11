using Prism.Commands;
using Prism.Ioc;
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
        private string _title = "Prism Application";
        private ObservableCollection<MenuBar> menuBars;
        private readonly IContainerProvider _containerProvider;
        private readonly IRegionManager _regionManager;
        private readonly IPlaylistService _playlistService;
        private readonly IPlayService _playService;
        private IRegionNavigationJournal _journal = null!;

        private UserData _userData = default!;

        public UserData UserData
        {
            get { return _userData; }
            set { SetProperty(ref _userData, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<MenuBar> MenuBars
        {
            get { return menuBars; }
            set { menuBars = value; RaisePropertyChanged(); }
        }

        private MenuBar? _navigateCurrentItem;

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

        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoForwardCommand { get; private set; }
        public DelegateCommand LogoutCommand { get; private set; }
        public DelegateCommand<ContentControl> OpenPlayViewCommand { get; private set; }
        public DelegateCommand<string> SearchCommand { get; private set; }
        public DelegateCommand LoginCommand { get; private set; }

        private DelegateCommand _playCommand = default!;

        public DelegateCommand PlayCommand =>
            _playCommand ?? (_playCommand = new DelegateCommand(_playService.Play));

        private DelegateCommand _pauseCommand = default!;

        public DelegateCommand PauseCommand =>
            _pauseCommand ?? (_pauseCommand = new DelegateCommand(_playService.Pause));

        private DelegateCommand _nextCommand = default!;

        public DelegateCommand NextCommand =>
            _nextCommand ?? (_nextCommand = new DelegateCommand(_playService.Next));

        private DelegateCommand _previousCommand = default!;

        public DelegateCommand PreviousCommand =>
            _previousCommand ?? (_previousCommand = new DelegateCommand(_playService.Previous));

        private bool _isPlaying = false;

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set { SetProperty(ref _isPlaying, value); }
        }

        private Song? _currentSong = null;

        public Song? CurrentSong
        {
            get { return _currentSong; }
            set { SetProperty(ref _currentSong, value); }
        }

        private double _songDuration = 1d;

        public double SongDuration
        {
            get { return _songDuration; }
            set { SetProperty(ref _songDuration, value); }
        }

        private double _songPosition = 0d;

        public double SongPosition
        {
            get { return _songPosition; }
            set { SetProperty(ref _songPosition, value); }
        }

        private double _volume = 0.5;

        public double Volume
        {
            get { return _volume; }
            set { SetProperty(ref _volume, value); }
        }

        private DelegateCommand<double?> _setVolumeCommand = default!;

        public DelegateCommand<double?> SetVolumeCommand =>
            _setVolumeCommand ?? (_setVolumeCommand = new DelegateCommand<double?>((value) =>
            {
                if (value.HasValue)
                {
                    _playService.SetVolume(value.Value);
                }
            }));

        private bool _isMuted = false;

        public bool IsMuted
        {
            get { return _isMuted; }
            set { SetProperty(ref _isMuted, value); }
        }

        private DelegateCommand<bool?> _setMutedCommand = default!;

        public DelegateCommand<bool?> SetMutedCommand =>
            _setMutedCommand ?? (_setMutedCommand = new DelegateCommand<bool?>(ExecuteSetMutedCommand));

        private void ExecuteSetMutedCommand(bool? parameter)
        {
            if (parameter.HasValue)
            {
                _playService.SetMute(parameter.Value);
            }
        }

        private DelegateCommand _playingListSwichCommand = default!;
        public DelegateCommand PlayingListSwichCommand =>
            _playingListSwichCommand ?? (_playingListSwichCommand = new DelegateCommand(ExecutePlayingListSwichCommand));

        void ExecutePlayingListSwichCommand()
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
        }

        public MainWindowViewModel(IContainerProvider containerProvider,
            IRegionManager regionManager, IPlaylistService playlistService, IPlayService playService)
        {
            _regionManager = regionManager;
            _containerProvider = containerProvider;
            menuBars = new ObservableCollection<MenuBar>();
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            GoBackCommand = new DelegateCommand(() =>
            {
                if (_journal != null && _journal.CanGoBack)
                    _journal.GoBack();
            });
            GoForwardCommand = new DelegateCommand(() =>
            {
                if (_journal != null && _journal.CanGoForward)
                    _journal.GoForward();
            });
            LogoutCommand = new DelegateCommand(Logout);
            LoginCommand = new DelegateCommand(Login);
            OpenPlayViewCommand = new DelegateCommand<ContentControl>(OpenPlayView);
            SearchCommand = new DelegateCommand<string>(Search);
            _playlistService = playlistService;
            _userData = UserData.Instance;
            _playService = playService;
            _playService.IsPlayingChanged += (s, e) => IsPlaying = e.IsPlaying;
            _playService.CurrentChanged += (s, e) => CurrentSong = e.NewSong;
            _playService.ProgressChanged += (s, e) =>
            {
                SongPosition = e.Value;
                SongDuration = e.Total;
            };
            Volume = _playService.Volume;
            _playService.VolumeChanged += (s, e) => Volume = e.Value;
            _playService.IsMutedChanged += (s, e) => IsMuted = e.NewValue;
        }

        private void Search(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText)) return;
            var parameters = new NavigationParameters();
            parameters.Add("SearchText", searchText);

            var regions = _regionManager.Regions[PrismManager.MainViewRegionName];

            var uri = _journal.CurrentEntry.Uri;   // TODO 导航BUG 待修复
            regions.RequestNavigate("SearchView", parameters);
        }

        private void OpenPlayView(ContentControl obj)
        {
            if (obj.Content is PlayView playView)
            {
                ((PlayViewModel)playView.DataContext).Display = true;
            }
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

        private void Login()
        {
            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(nameof(LoginView));
        }

        private void CreateMenuBar()
        {
            MenuBars.Add(new MenuBar() { Icon = "Home", Title = "首页", NameSpace = "IndexView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookOutline", Title = "发现", NameSpace = "FoundView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookPlus", Title = "音乐库", NameSpace = "LibraryView" });
        }

        private void Navigate(MenuBar obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.NameSpace))
                return;

            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.NameSpace);
        }

        private void NavigationService_Navigated(object? sender, RegionNavigationEventArgs e)
        {
            var viewName = e.Uri.OriginalString;
            var menuBar = menuBars.FirstOrDefault(x => x.NameSpace == viewName);

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

        /// <summary>
        /// 配置首页初始化参数
        /// </summary>
        public async void Configure()
        {
            CreateMenuBar();

            var response = await _playlistService.LoginStatus();
            if (response.Data.Code == 200)
            {
                UserData.Cover = response.Data.Profile?.AvatarUrl;
                UserData.NickName = response.Data.Profile?.Nickname;
                UserData.Id = response.Data.Account?.Id ?? 0;
                UserData.VipType = response.Data.Account?.VipType ?? 0;
                UserData.Save();
            }

            _regionManager.Regions[PrismManager.MainViewRegionName].NavigationService.Navigated += NavigationService_Navigated;
            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(MenuBars[0].NameSpace, back =>
            {
                _journal = back.Context.NavigationService.Journal;
            });

            _regionManager.Regions[PrismManager.FullScreenRegionName].RequestNavigate("PlayView", (result) =>
            {
            });

            _playService.Add(new Song
            {
                Id = 493735159
            });
        }
    }
}