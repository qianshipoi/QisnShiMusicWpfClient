using Prism.Ioc;

using QianShi.Music.ViewModels;

namespace QianShi.Music.Common
{
    public class ViewModelLocator
    {
        public SearchViewModel SearchViewModel => App.Current.Container.Resolve<SearchViewModel>();
        public AlbumViewModel AlbumViewModel => App.Current.Container.Resolve<AlbumViewModel>();
        public ArtistViewModel ArtistViewModel => App.Current.Container.Resolve<ArtistViewModel>();
        public FondPlaylistViewModel FondPlaylistViewModel => App.Current.Container.Resolve<FondPlaylistViewModel>();
        public FoundViewModel FoundViewModel => App.Current.Container.Resolve<FoundViewModel>();
        public IndexViewModel IndexViewModel => App.Current.Container.Resolve<IndexViewModel>();
        public LibraryViewModel LibraryViewModel => App.Current.Container.Resolve<LibraryViewModel>();
        public LoginViewModel LoginViewModel => App.Current.Container.Resolve<LoginViewModel>();
        public MainWindowViewModel MainWindowViewModel => App.Current.Container.Resolve<MainWindowViewModel>();
        public MvViewModel MvViewModel => App.Current.Container.Resolve<MvViewModel>();
        public PlayingListViewModel PlayingListViewModel => App.Current.Container.Resolve<PlayingListViewModel>();
        public PlaylistCardViewModel PlaylistCardViewModel => App.Current.Container.Resolve<PlaylistCardViewModel>();
        public PlaylistViewModel PlaylistViewModel => App.Current.Container.Resolve<PlaylistViewModel>();
        public PlayViewModel PlayViewModel => App.Current.Container.Resolve<PlayViewModel>();
        public SearchDetailViewModel SearchDetailViewModel => App.Current.Container.Resolve<SearchDetailViewModel>();
        public VideoPlayWindowViewModel VideoPlayWindowViewModel => App.Current.Container.Resolve<VideoPlayWindowViewModel>();
    }
}
