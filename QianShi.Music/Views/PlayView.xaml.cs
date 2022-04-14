using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
