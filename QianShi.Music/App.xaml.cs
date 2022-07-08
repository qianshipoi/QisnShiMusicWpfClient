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
        private readonly string _cookieSavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cookie.json");
        private TaskbarIcon? _tbi = null;
        private IPlaylistService _playlistService = default!;
        private ISnackbarMessageQueue _snackbarMessageQueue = default!;

        public new static App Current => (App)Application.Current;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // UI线程未捕获异常
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            // Task线程未捕获异常
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            // 非UI线程未捕获异常
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var sb = new StringBuilder();
            if (e.IsTerminating)
            {
                sb.Append("非UI线程发生错误");
            }
            sb.Append("非UI线程异常：");
            if (e.ExceptionObject is Exception exception)
            {
                sb.Append(exception.Message);
            }
            else
            {
                sb.Append(e.ExceptionObject);
            }
            _snackbarMessageQueue.Enqueue(sb.ToString(), TimeSpan.FromSeconds(5));
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            _snackbarMessageQueue.Enqueue("Task线程异常：" + e.Exception.Message, TimeSpan.FromSeconds(5));
            e.SetObserved();
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                e.Handled = true;
                _snackbarMessageQueue.Enqueue("UI线程异常：" + e.Exception.Message, TimeSpan.FromSeconds(5));
            }
            catch
            {
                _snackbarMessageQueue.Enqueue("UI线程发生致命错误！：" + e.Exception.Message, TimeSpan.FromSeconds(5));
            }
        }

        protected override Window CreateShell()
        {
            _playlistService = Container.Resolve<IPlaylistService>();
            _snackbarMessageQueue = Container.Resolve<ISnackbarMessageQueue>();
            if (File.Exists(_cookieSavePath))
            {
                var cookiesJson = File.ReadAllText(_cookieSavePath);
                JsonSerializer.Deserialize<List<CookieInfo>>(cookiesJson)?.ToList().ForEach(x => _playlistService.SetCookie(x.ToCookie()));
            }

            _tbi = (TaskbarIcon)FindResource("NotifyIcon");
            return Container.Resolve<MainWindow>();
            //return new TestControlsWindows();
        }

        protected override void OnInitialized()
        {
            (MainWindow.DataContext as IConfigureService)?.Configure();
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

            containerRegistry.RegisterSingleton<ISnackbarMessageQueue, SnackbarMessageQueue>();

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