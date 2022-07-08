using QianShi.Music.Common;
using QianShi.Music.Common.Models.Response;
using QianShi.Music.Data;
using QianShi.Music.Models;
using QianShi.Music.Services;

namespace QianShi.Music.ViewModels
{
    public class ArtistViewModel : NavigationViewModel, IRegionMemberLifetime
    {
        public const string ArtistIdParameterName = "ArtistId";

        private readonly INavigationService _navigationService;
        private readonly IPlaylistService _playlistService;
        private readonly IDataProvider<ArtistModel, long> _dataProvider;
        private Album? _album;
        private Artist _artist = default!;
        private long _artistId;
        private DelegateCommand<MovieVideo> _jumpToMvPageCommand = default!;
        private MovieVideo? _movieVideo;
        private DelegateCommand<IPlaylist> _openArtistCommand = default!;
        private DelegateCommand<Album> _openPlaylistCommand = default!;

        public ArtistViewModel(
            IDataProvider<ArtistModel, long> dataProvider,
            IContainerProvider containerProvider,
            IPlaylistService playlistService,
            INavigationService navigationService)
            : base(containerProvider)
        {
            _dataProvider = dataProvider;
            _playlistService = playlistService;
            _navigationService = navigationService;
        }

        public ObservableCollection<Album> Albums { get; } = new();
        public ObservableCollection<Artist> Artists { get; } = new();
        public ObservableCollection<MovieVideo> MovieVideos { get; } = new();
        public ObservableCollection<Song> Songs { get; } = new();

        public Album? Album
        {
            get => _album;
            set => SetProperty(ref _album, value);
        }

        public Artist Artist
        {
            get => _artist;
            set => SetProperty(ref _artist, value);
        }

        public bool KeepAlive => false;

        public MovieVideo? MovieVideo
        {
            get => _movieVideo;
            set => SetProperty(ref _movieVideo, value);
        }

        public DelegateCommand<MovieVideo> JumpToMvPageCommand =>
            _jumpToMvPageCommand ??= new((mv) => _navigationService.NavigateToMv(mv.Id));

        public DelegateCommand<IPlaylist> OpenArtistCommand =>
            _openArtistCommand ??= new((playlist) => _navigationService.NavigateToArtist(playlist.Id));

        public DelegateCommand<Album> OpenPlaylistCommand =>
            _openPlaylistCommand ??= new((album) => _navigationService.NavigateToAlbum(album.Id));

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey(ArtistIdParameterName))
            {
                return false;
            }

            return base.IsNavigationTarget(navigationContext);
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            var parameters = navigationContext.Parameters;
            var artistId = parameters.GetValue<long>(ArtistIdParameterName);
            if (_artistId != artistId)
            {
                _artistId = artistId;
                IsBusy = true;
                try
                {
                    var result = await _dataProvider.GetDataAsync(_artistId);
                    if (result == null)
                    {
                        navigationContext.NavigationService.Journal.GoBack();
                        return;
                    }

                    Artist = result.Artist;
                    Songs.AddRange(result.Songs ?? new());
                    if (Albums.Count > 0)
                    {
                        Albums.AddRange(result.Albums);
                        Album = Albums[0];
                    }
                    if (MovieVideos.Count > 0)
                    {
                        MovieVideos.AddRange(result.MovieVideos);
                        MovieVideo = MovieVideos[0];
                    }
                    Artists.AddRange(result.Artists ?? new());
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }
    }
}