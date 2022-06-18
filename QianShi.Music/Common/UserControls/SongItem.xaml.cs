using QianShi.Music.Common.Models.Response;
using QianShi.Music.Services;

namespace QianShi.Music.Common.UserControls
{
    /// <summary>
    /// SongItem.xaml 的交互逻辑
    /// </summary>
    public partial class SongItem
    {
        public static readonly DependencyProperty PlayImmediatelyCommandProperty =
            DependencyProperty.Register(nameof(PlayImmediatelyCommand), typeof(ICommand), typeof(SongItem), new PropertyMetadata(null));

        public static readonly DependencyProperty PlaylistItemProperty =
            DependencyProperty.Register(nameof(Item), typeof(Song), typeof(SongItem), new PropertyMetadata(new Song
            {
                Album = new Album
                {
                    CoverImgUrl = "https://oss.kuriyama.top/static/background.png"
                }
            }));

        private DelegateCommand<IPlaylist> _jumpToPlaylistCommand = default!;

        public SongItem()
        {
            InitializeComponent();
        }

        public Song Item
        {
            get => (Song)GetValue(PlaylistItemProperty);
            set => SetValue(PlaylistItemProperty, value);
        }

        public DelegateCommand<IPlaylist> JumpToPlayListCommand =>
            _jumpToPlaylistCommand ??= new DelegateCommand<IPlaylist>(ExecuteJumpToPlayListCommand);

        public ICommand PlayImmediatelyCommand
        {
            get => (ICommand)GetValue(PlayImmediatelyCommandProperty);
            set => SetValue(PlayImmediatelyCommandProperty, value);
        }

        private INavigationService _navigationService => App.Current.Container.Resolve<INavigationService>();

        private void ExecuteJumpToPlayListCommand(IPlaylist parameter)
        {
            switch (parameter)
            {
                case Album album:
                    _navigationService.NavigateToAlbum(album.Id);
                    break;

                case Artist artist:
                    _navigationService.NavigateToArtist(artist.Id);
                    break;

                default:
                    break;
            }
        }
    }
}