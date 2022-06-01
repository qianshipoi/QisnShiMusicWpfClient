using QianShi.Music.Common.Helpers;

using System.Windows;
using System.Windows.Controls.Primitives;

namespace QianShi.Music.Views
{
    /// <summary>
    /// VideoPlayWindow.xaml 的交互逻辑
    /// </summary>
    public partial class VideoPlayWindow : Window
    {
        public VideoPlayWindow()
        {
            InitializeComponent();
            Loaded += VideoPlayWindow_Loaded;
        }

        private void VideoPlayWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FullScreenHelper.StartFullScreen(this);
        }

        private void SwitchWindowSize(object sender, RoutedEventArgs e)
        {
            var toggleButton = (ToggleButton)sender;

            if (toggleButton.IsChecked is true)
            {
                FullScreenHelper.StartFullScreen(this);
            }
            else
            {
                FullScreenHelper.EndFullScreen(this);
            }
        }
    }
}