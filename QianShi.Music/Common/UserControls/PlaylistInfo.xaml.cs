using QianShi.Music.ViewModels;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QianShi.Music.Common.UserControls
{
    /// <summary>
    /// PlaylistInfo.xaml 的交互逻辑
    /// </summary>
    public partial class PlaylistInfo : UserControl
    {
        public PlaylistDetail Detail
        {
            get { return (PlaylistDetail)GetValue(DetailProperty); }
            set { SetValue(DetailProperty, value); }
        }

        public static readonly DependencyProperty DetailProperty =
            DependencyProperty.Register(nameof(Detail), typeof(PlaylistDetail), typeof(PlaylistInfo), new PropertyMetadata(null));

        public ICommand ShowDescriptionCommand
        {
            get { return (ICommand)GetValue(ShowDescriptionCommandProperty); }
            set { SetValue(ShowDescriptionCommandProperty, value); }
        }

        public static readonly DependencyProperty ShowDescriptionCommandProperty =
            DependencyProperty.Register(nameof(ShowDescriptionCommand), typeof(ICommand), typeof(PlaylistInfo), new PropertyMetadata(null));

        public ICommand PlayCommand
        {
            get { return (ICommand)GetValue(PlayCommandProperty); }
            set { SetValue(PlayCommandProperty, value); }
        }

        public static readonly DependencyProperty PlayCommandProperty =
            DependencyProperty.Register(nameof(PlayCommand), typeof(ICommand), typeof(PlaylistInfo), new PropertyMetadata(null));

        public PlaylistInfo()
        {
            InitializeComponent();
        }
    }
}