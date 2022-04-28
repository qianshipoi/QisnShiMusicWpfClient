using QianShi.Music.Common.Models.Response;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QianShi.Music.Common.UserControls
{
    /// <summary>
    /// SongItem.xaml 的交互逻辑
    /// </summary>
    public partial class SongItem : UserControl
    {
        public ICommand PlayImmediatelyCommand
        {
            get { return (ICommand)GetValue(PlayImmediatelyCommandProperty); }
            set { SetValue(PlayImmediatelyCommandProperty, value); }
        }

        public static readonly DependencyProperty PlayImmediatelyCommandProperty =
            DependencyProperty.Register(nameof(PlayImmediatelyCommand), typeof(ICommand), typeof(SongItem), new PropertyMetadata(null));

        public Song Item
        {
            get { return (Song)GetValue(PlaylistItemProperty); }
            set { SetValue(PlaylistItemProperty, value); }
        }

        public static readonly DependencyProperty PlaylistItemProperty =
            DependencyProperty.Register(nameof(Item), typeof(Song), typeof(SongItem), new PropertyMetadata(new Song
            {
                Album = new Album
                {
                    CoverImgUrl = "https://oss.kuriyama.top/static/background.png"
                }
            }));

        public SongItem()
        {
            InitializeComponent();
        }
    }
}
