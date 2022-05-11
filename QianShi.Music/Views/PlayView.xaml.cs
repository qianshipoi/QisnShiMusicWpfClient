using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace QianShi.Music.Views
{
    /// <summary>
    /// PlayView.xaml 的交互逻辑
    /// </summary>
    public partial class PlayView : UserControl
    {
        private Window _targetWindow = null!;

        public PlayView()
        {
            InitializeComponent();
            Loaded += PlayView_Loaded;
        }

        private void PlayView_Loaded(object sender, RoutedEventArgs e)
        {
            _targetWindow = Window.GetWindow(this);
            //Margin = new Thickness(0, _targetWindow.Height, 0, 0);
        }

        private void PlayView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _targetWindow!.DragMove();
            }
        }
    }
}