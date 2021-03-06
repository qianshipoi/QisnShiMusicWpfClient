using QianShi.Music.Common.Models;

namespace QianShi.Music.Common.UserControls
{
    /// <summary>
    /// PlaylistListItem.xaml 的交互逻辑
    /// </summary>
    public partial class PlaylistListItem : UserControl
    {
        public ICommand PlayImmediatelyCommand
        {
            get { return (ICommand)GetValue(PlayImmediatelyCommandProperty); }
            set { SetValue(PlayImmediatelyCommandProperty, value); }
        }

        public static readonly DependencyProperty PlayImmediatelyCommandProperty =
            DependencyProperty.Register(nameof(PlayImmediatelyCommand), typeof(ICommand), typeof(PlaylistListItem), new PropertyMetadata(null));

        public PlaylistItem Item
        {
            get { return (PlaylistItem)GetValue(PlaylistItemProperty); }
            set { SetValue(PlaylistItemProperty, value); }
        }

        public static readonly DependencyProperty PlaylistItemProperty =
            DependencyProperty.Register(nameof(Item), typeof(PlaylistItem), typeof(PlaylistListItem), new PropertyMetadata(new PlaylistItem
            {
                PicUrl = "https://oss.kuriyama.top/static/background.png"
            }));

        public PlaylistListItem()
        {
            InitializeComponent();
        }
    }
}