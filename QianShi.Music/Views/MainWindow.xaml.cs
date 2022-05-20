using QianShi.Music.Services;

using System.Windows;
using Prism.Ioc;

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

            btnMin.Click += (s, e) => { this.WindowState = WindowState.Minimized; };
            btnMax.Click += (s, e) =>
            {
                if (this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
                else
                {
                    this.WindowState = WindowState.Maximized;
                }
            };
            btnClose.Click += (s, e) =>
           {
               //var dialogResult = await dialogHostService.Question("温馨提示", "确认退出系统?");
               //if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;
               this.Close();
           };
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