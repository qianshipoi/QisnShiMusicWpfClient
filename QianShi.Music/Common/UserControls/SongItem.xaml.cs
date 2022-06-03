using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

using QianShi.Music.Common.Models.Response;
using QianShi.Music.Extensions;
using QianShi.Music.ViewModels;
using QianShi.Music.Views;

using System.Windows;
using System.Windows.Input;

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

        private DelegateCommand<Album> _jumpToAlbumPageCommand = default!;
        private DelegateCommand<Artist> _jumpToArtistPageCommand = default!;
        public SongItem()
        {
            InitializeComponent();
        }

        public Song Item
        {
            get => (Song)GetValue(PlaylistItemProperty);
            set => SetValue(PlaylistItemProperty, value);
        }

        public DelegateCommand<Album> JumpToAlbumPageCommand =>
            _jumpToAlbumPageCommand ??= new(ExecuteJumpToAlbumPageCommand);

        public DelegateCommand<Artist> JumpToArtistPageCommand =>
            _jumpToArtistPageCommand ??= new(ExecuteJumpToArtistPageCommand);

        public ICommand PlayImmediatelyCommand
        {
            get => (ICommand)GetValue(PlayImmediatelyCommandProperty);
            set => SetValue(PlayImmediatelyCommandProperty, value);
        }

        private IRegionManager _regionManager => App.Current.Container.Resolve<IRegionManager>();
        private void ExecuteJumpToAlbumPageCommand(Album album)
        {
            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(nameof(AlbumView), new NavigationParameters
            {
                {AlbumViewModel.AlbumIdParameterName,album.Id}
            });
        }

        private void ExecuteJumpToArtistPageCommand(Artist artist)
        {
            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(nameof(ArtistView), new NavigationParameters
            {
                {ArtistViewModel.ArtistIdParameterName,artist.Id}
            });
        }
    }
}