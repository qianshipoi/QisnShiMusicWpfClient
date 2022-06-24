using QianShi.Music.Common.Models;
using QianShi.Music.Extensions;
using QianShi.Music.Services;

namespace QianShi.Music.ViewModels.Navigation
{
    public class NavigationBarViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        private readonly IRegionManager _regionManager;

        private MenuBar? _navigateCurrentItem;
        private bool _popupIsOpen;
        private DelegateCommand _goForwardCommand = default!;
        private DelegateCommand _goBackCommand = default!;
        private DelegateCommand<string> _searchCommand = default!;
        private DelegateCommand _loginCommand = default!;
        private DelegateCommand _settingCommand = default!;
        private DelegateCommand _logoutCommand = default!;
        private DelegateCommand _minimizedCommand = default!;
        private DelegateCommand _maximizedCommand = default!;
        private DelegateCommand _closeCommand = default!;

        public ObservableCollection<MenuBar> MenuBars { get; } = new();

        public UserData UserData => UserData.Instance;

        public bool PopupIsOpen
        {
            get => _popupIsOpen;
            set => SetProperty(ref _popupIsOpen, value);
        }

        public MenuBar? NavigateCurrentItem
        {
            get => _navigateCurrentItem;
            set
            {
                if (_navigateCurrentItem != value && null != value)
                {
                    if (string.IsNullOrWhiteSpace(value.NameSpace))
                        return;
                    _navigationService.MainRegionNavigation(value.NameSpace);
                }
                SetProperty(ref _navigateCurrentItem, value);
            }
        }

        public DelegateCommand GoForwardCommand =>
            _goForwardCommand ??= new(_navigationService.GoForward);

        public DelegateCommand GoBackCommand =>
            _goBackCommand ??= new(_navigationService.GoBack);

        public DelegateCommand<string> SearchCommand =>
            _searchCommand ??= new((keywords) =>
            {
                if (!string.IsNullOrWhiteSpace(keywords))
                {
                    _navigationService.NavigateToSearch(keywords.Trim());
                }
            });

        public DelegateCommand LoginCommand =>
            _loginCommand ??= new(_navigationService.NavigateToLogin);

        public DelegateCommand SettingCommand =>
            _settingCommand ??= new(_navigationService.NavigateToSetting);

        public DelegateCommand LogoutCommand =>
            _logoutCommand ??= new(async () => await UserData.LogoutAsync());

        private Window MainWindow => App.Current.MainWindow;

        public DelegateCommand MinimizedCommand =>
            _minimizedCommand ??= new(() => MainWindow.WindowState = WindowState.Minimized);

        public DelegateCommand MaximizedCommand =>
            _maximizedCommand ??= new(() => MainWindow.WindowState = MainWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized);

        public DelegateCommand CloseCommand =>
            _closeCommand ??= new(MainWindow.Close);

        public NavigationBarViewModel(
            INavigationService navigationService,
            IRegionManager regionManager)
        {
            _navigationService = navigationService;
            _regionManager = regionManager;

            CreateMenuBar();

            _regionManager.Regions[PrismManager.MainViewRegionName].NavigationService.Navigated += NavigationService_Navigated;

            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(MenuBars[0].NameSpace);
        }

        private void NavigationService_Navigated(object? sender, RegionNavigationEventArgs e)
        {
            var viewName = e.Uri.OriginalString;
            var menuBar = MenuBars.FirstOrDefault(x => x.NameSpace == viewName);

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

        private void CreateMenuBar()
        {
            MenuBars.Add(new MenuBar() { Icon = "Home", Title = "首页", NameSpace = "IndexView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookOutline", Title = "发现", NameSpace = "FoundView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookPlus", Title = "音乐库", NameSpace = "LibraryView", Auth = true });
        }
    }
}
