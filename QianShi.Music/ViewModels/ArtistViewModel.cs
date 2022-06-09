using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

using QianShi.Music.Common;
using QianShi.Music.Common.Models.Response;
using QianShi.Music.Services;

using System.Collections.ObjectModel;

namespace QianShi.Music.ViewModels
{
    public class ArtistViewModel : NavigationViewModel, IRegionMemberLifetime
    {
        public const string ArtistIdParameterName = "ArtistId";

        private readonly INavigationService _navigationService;
        private readonly IPlaylistService _playlistService;
        private Album? _album;
        private Artist _artist = default!;
        private long _artistId;
        private DelegateCommand<MovieVideo> _jumpToMvPageCommand = default!;
        private bool _loading;
        private MovieVideo? _movieVideo;
        private DelegateCommand<IPlaylist> _openArtistCommand = default!;
        private DelegateCommand<Album> _openPlaylistCommand = default!;

        public ArtistViewModel(
            IContainerProvider containerProvider,
            IPlaylistService playlistService,
            INavigationService navigationService)
            : base(containerProvider)
        {
            _playlistService = playlistService;
            _navigationService = navigationService;
        }

        public Album? Album
        {
            get => _album;
            set => SetProperty(ref _album, value);
        }

        public ObservableCollection<Album> Albums { get; } = new();

        public Artist Artist
        {
            get => _artist;
            set => SetProperty(ref _artist, value);
        }

        public ObservableCollection<Artist> Artists { get; } = new();

        public DelegateCommand<MovieVideo> JumpToMvPageCommand =>
            _jumpToMvPageCommand ??= new((mv) => _navigationService.NavigateToMv(mv.Id));

        public bool KeepAlive => false;

        public bool Loading
        {
            get => _loading;
            set => SetProperty(ref _loading, value);
        }

        public MovieVideo? MovieVideo
        {
            get => _movieVideo;
            set => SetProperty(ref _movieVideo, value);
        }

        public ObservableCollection<MovieVideo> MovieVideos { get; } = new();

        public DelegateCommand<IPlaylist> OpenArtistCommand =>
            _openArtistCommand ??= new((playlist) => _navigationService.NavigateToArtist(playlist.Id));

        public DelegateCommand<Album> OpenPlaylistCommand =>
            _openPlaylistCommand ??= new((album) => _navigationService.NavigateToAlbum(album.Id));

        public ObservableCollection<Song> Songs { get; } = new();

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

            _artistId = parameters.GetValue<long>(ArtistIdParameterName);
            Loading = true;
            try
            {
                await GetHotSongs();
                await GetAlbums();
                await GetMovieVideos();
                await GetArtists();
            }
            finally
            {
                Loading = false;
            }
        }

        private async Task GetAlbums()
        {
            var response = await _playlistService.ArtistAlbum(new Common.Models.Request.ArtistAlbumRequest
            {
                Id = _artistId,
                Limit = 20
            });
            if (response.Code == 200)
            {
                var albums = response.HotAlbums.Select(x =>
                {
                    x.CoverImgUrl += "?param=200y200";
                    return x;
                }).ToList();
                if (albums.Any())
                {
                    Albums.AddRange(albums);
                    Album = albums[0];
                }
            }
        }

        private async Task GetArtists()
        {
            var response = await _playlistService.SimiArtist(_artistId);
            if (response.Code == 200)
            {
                Artists.AddRange(response.Artists.Take(12).Select(x =>
                {
                    x.CoverImgUrl += "?param=200y200";
                    return x;
                }));
            }
        }

        private async Task GetHotSongs()
        {
            var response = await _playlistService.Artists(_artistId);
            if (response.Code == 200)
            {
                Artist = response.Artist;

                Songs.AddRange(response.HotSongs.Select(x =>
                {
                    x.Album.CoverImgUrl += "?param=48y48";
                    return x;
                }).Take(12));
            }
        }

        private async Task GetMovieVideos()
        {
            var response = await _playlistService.ArtistMv(new Common.Models.Request.ArtistMvRequest
            {
                Id = _artistId,
                Limit = 20
            });
            if (response.Code == 200)
            {
                var mvs = response.Mvs.Select(x =>
                {
                    x.CoverImgUrl += "?param=464y260";
                    return x;
                }).ToList();

                if (mvs.Any())
                {
                    MovieVideos.AddRange(mvs);
                    MovieVideo = mvs[0];
                }
            }
        }
    }
}