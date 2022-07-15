using System.Windows.Controls;

namespace QianShi.Music.Views
{
    /// <summary>
    /// FoundView.xaml 的交互逻辑
    /// </summary>
    public partial class FoundView : UserControl
    {
        public FoundView()
        {
            InitializeComponent();
            ScrollViewerControl.ScrollChanged += ScrollViewerControl_ScrollChanged;
        }

        private void ScrollViewerControl_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalOffset > 200)
            {
                BackTopButton.Visibility = Visibility.Visible;
            }
            else
            {
                BackTopButton.Visibility = Visibility.Collapsed;
            }
        }

        private void BackTopButton_Click(object sender, RoutedEventArgs e)
        {
            //ScrollViewerControl.ScrollToTop();
            ScrollViewerControl.GoBackTop();
        }
    }
}