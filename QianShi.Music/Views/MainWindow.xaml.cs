using QianShi.Music.Services;

namespace QianShi.Music.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IPlaylistService _playlistService;
        private readonly IContainerProvider _containerProvider;

        public MainWindow(IPlaylistService playlistService, IContainerProvider containerProvider)
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            _playlistService = playlistService;
            _containerProvider = containerProvider;
        }

        protected override void OnClosed(EventArgs e)
        {
            var videoPlayWindow = _containerProvider.Resolve<VideoPlayWindow>();
            videoPlayWindow.Close();
            base.OnClosed(e);
        }
    }
}