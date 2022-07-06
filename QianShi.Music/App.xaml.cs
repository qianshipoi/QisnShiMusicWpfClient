using Hardcodet.Wpf.TaskbarNotification;

using QianShi.Music.Common;
using QianShi.Music.Common.Models;
using QianShi.Music.Extensions;
using QianShi.Music.Services;
using QianShi.Music.ViewModels;
using QianShi.Music.ViewModels.Dialogs;
using QianShi.Music.ViewModels.Navigation;
using QianShi.Music.Views;
using QianShi.Music.Views.Dialogs;
using QianShi.Music.Views.Navigation;

namespace QianShi.Music
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private TaskbarIcon? _tbi = null;
        public new static App Current => (App)Application.Current;

        private IPlaylistService _playlistService = default!;
        private readonly string _cookieSavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cookie.json");

        protected override Window CreateShell()
        {
            _playlistService = Container.Resolve<IPlaylistService>();
            if (File.Exists(_cookieSavePath))
            {
                var cookiesJson = File.ReadAllText(_cookieSavePath);
                var cookies = JsonSerializer.Deserialize<List<CookieInfo>>(cookiesJson);
                if (null != cookies)
                {
                    foreach (var cookie in cookies)
                    {
                        _playlistService.SetCookie(cookie.ToCookie());
                    }
                }
            }
            _tbi = (TaskbarIcon)FindResource("NotifyIcon");
            return Container.Resolve<MainWindow>();
            //return new TestControlsWindows();
        }

        protected override void OnInitialized()
        {
            var service = App.Current.MainWindow.DataContext as IConfigureService;
            if (service != null)
                service.Configure();

            Container.Resolve<IPreferenceService>()?.InitTheme();

            base.OnInitialized();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IPlaylistService, PlaylistService>();
            containerRegistry.RegisterSingleton<IPlayService, MediaPlayerPlayService>();
            containerRegistry.RegisterSingleton<IPlayStoreService, PlayStoreService>();
            containerRegistry.RegisterSingleton<IVideoPlayService, MediaElementPlayService>();
            containerRegistry.RegisterSingleton<IVideoPlayStoreService, VideoPlayStoreService>();
            containerRegistry.RegisterSingleton<IPlaylistStoreService, PlaylistStoreService>();
            containerRegistry.RegisterSingleton<INavigationService, NavigationService>();
            containerRegistry.RegisterSingleton<IPreferenceService, JsonFilePreferenceService>();

            containerRegistry.Register<VideoPlayWindow>();

            containerRegistry.RegisterForNavigation<DescriptionDialog, DescriptionDialogViewModel>();
            containerRegistry.RegisterForNavigation<IndexView, IndexViewModel>();
            containerRegistry.RegisterForNavigation<FoundView, FoundViewModel>();
            containerRegistry.RegisterForNavigation<LibraryView, LibraryViewModel>();
            containerRegistry.RegisterForNavigation<PlaylistView, PlaylistViewModel>();
            containerRegistry.RegisterForNavigation<PlayView, PlayViewModel>();
            containerRegistry.RegisterForNavigation<PlaylistCardView, PlaylistCardViewModel>();
            containerRegistry.RegisterForNavigation<AlbumView, AlbumViewModel>();
            containerRegistry.RegisterForNavigation<SearchView, SearchViewModel>();
            containerRegistry.RegisterForNavigation<SearchDetailView, SearchDetailViewModel>();
            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNavigation<ArtistView, ArtistViewModel>();
            containerRegistry.RegisterForNavigation<PlayingListView, PlayingListViewModel>();
            containerRegistry.RegisterForNavigation<MvView, MvViewModel>();
            containerRegistry.RegisterForNavigation<FondPlaylistView, FondPlaylistViewModel>();
            containerRegistry.RegisterForNavigation<SettingView, SettingViewModel>();
            containerRegistry.RegisterForNavigation<NavigationBarView, NavigationBarViewModel>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            var cookies = _playlistService.GetCookieCollection();
            if (cookies != null)
            {
                var cookiesData = JsonSerializer.Serialize(cookies.Select(i => new CookieInfo(i)));

                File.WriteAllTextAsync(_cookieSavePath, cookiesData);
            }
            base.OnExit(e);
        }
    }
}