using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QianShi.Music.Common.UserControls
{
    /// <summary>
    /// PlaylistCard.xaml 的交互逻辑
    /// </summary>
    public partial class PlaylistCard : UserControl
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(PlaylistCard), new PropertyMetadata(string.Empty));

        public long AmountOfPlay
        {
            get { return (long)GetValue(AmountOfPlayProperty); }
            set { SetValue(AmountOfPlayProperty, value); }
        }

        public static readonly DependencyProperty AmountOfPlayProperty =
            DependencyProperty.Register(nameof(AmountOfPlay), typeof(long), typeof(PlaylistCard), new FrameworkPropertyMetadata(0L, (dp, e) =>
            {
                if (dp is PlaylistCard card)
                {
                    card.PlayCountControl.Visibility = e.NewValue == default ? Visibility.Collapsed : Visibility.Visible;
                }
            }));

        public string Cover
        {
            get { return (string)GetValue(CoverProperty); }
            set { SetValue(CoverProperty, value); }
        }

        public static readonly DependencyProperty CoverProperty =
            DependencyProperty.Register(nameof(Cover), typeof(string), typeof(PlaylistCard), new PropertyMetadata(string.Empty));

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(nameof(Description), typeof(string), typeof(PlaylistCard), new FrameworkPropertyMetadata(string.Empty, (dp, e) =>
            {
                if (dp is PlaylistCard card)
                {
                    card.DescriptionControl.Visibility = string.IsNullOrWhiteSpace(e.ToString()) ? Visibility.Collapsed : Visibility.Visible;
                }
            }));

        public IPlaylist Playlist
        {
            get { return (IPlaylist)GetValue(PlaylistProperty); }
            set { SetValue(PlaylistProperty, value); }
        }

        public static readonly DependencyProperty PlaylistProperty =
            DependencyProperty.Register(nameof(Playlist), typeof(IPlaylist), typeof(PlaylistCard), new PropertyMetadata(null));

        public ICommand OpenPlaylistCommand
        {
            get { return (ICommand)GetValue(OpenPlaylistCommandProperty); }
            set { SetValue(OpenPlaylistCommandProperty, value); }
        }

        public static readonly DependencyProperty OpenPlaylistCommandProperty =
            DependencyProperty.Register(nameof(OpenPlaylistCommand), typeof(ICommand), typeof(PlaylistCard), new PropertyMetadata(null));

        public PlaylistCard()
        {
            InitializeComponent();
        }
    }
}
