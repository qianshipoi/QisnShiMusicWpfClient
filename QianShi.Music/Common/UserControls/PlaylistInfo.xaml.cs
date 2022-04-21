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

namespace QianShi.Music.Common.UserControls
{
    /// <summary>
    /// PlaylistInfo.xaml 的交互逻辑
    /// </summary>
    public partial class PlaylistInfo : UserControl
    {


        public ICommand ShowDescriptionCommand
        {
            get { return (ICommand)GetValue(ShowDescriptionCommandProperty); }
            set { SetValue(ShowDescriptionCommandProperty, value); }
        }

        public static readonly DependencyProperty ShowDescriptionCommandProperty =
            DependencyProperty.Register(nameof(ShowDescriptionCommand), typeof(ICommand), typeof(PlaylistInfo), new PropertyMetadata(null));
        public ICommand PlayCommand
        {
            get { return (ICommand)GetValue(PlayCommandCommandProperty); }
            set { SetValue(PlayCommandCommandProperty, value); }
        }

        public static readonly DependencyProperty PlayCommandCommandProperty =
            DependencyProperty.Register(nameof(PlayCommand), typeof(ICommand), typeof(PlaylistInfo), new PropertyMetadata(null));

        public PlaylistInfo()
        {
            InitializeComponent();
        }
    }
}
