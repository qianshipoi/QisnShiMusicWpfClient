namespace QianShi.Music.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IContainerProvider _containerProvider;

        public MainWindow(IContainerProvider containerProvider)
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
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