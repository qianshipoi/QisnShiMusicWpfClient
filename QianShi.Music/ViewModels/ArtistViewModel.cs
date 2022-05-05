using System.Collections.ObjectModel;

using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

using QianShi.Music.Common;
using QianShi.Music.Common.Models.Response;
using QianShi.Music.Extensions;
using QianShi.Music.Services;
using QianShi.Music.Views;

namespace QianShi.Music.ViewModels
{
    public class ArtistViewModel : NavigationViewModel, IRegionMemberLifetime
    {
        public const string ArtistIdParameterName = "ArtistId";

        private readonly IPlaylistService _playlistService;
        private readonly IRegionManager _regionManager;
        private long ArtistId = 0;
        private ObservableCollection<Song> _songs;
        public ObservableCollection<Song> Songs
        {
            get { return _songs; }
            set { SetProperty(ref _songs, value); }
        }

        private ObservableCollection<Album> _albums;
        public ObservableCollection<Album> Albums
        {
            get { return _albums; }
            set
            {
                SetProperty(ref _albums, value);
            }
        }

        private ObservableCollection<MovieVideo> _movieVideos;
        public ObservableCollection<MovieVideo> MovieVideos
        {
            get => _movieVideos;
            set => SetProperty(ref _movieVideos, value);
        }

        private ObservableCollection<Artist> _artists;
        public ObservableCollection<Artist> Artists
        {
            get { return _artists; }
            set { SetProperty(ref _artists, value); }
        }

        private Artist _artist = default!;
        public Artist Artist
        {
            get { return _artist; }
            set { SetProperty(ref _artist, value); }
        }

        private Album? _album;
        public Album? Album
        {
            get { return _album; }
            set { SetProperty(ref _album, value); }
        }

        private MovieVideo? _movieVideo;
        public MovieVideo? MovieVideo
        {
            get { return _movieVideo; }
            set { SetProperty(ref _movieVideo, value); }
        }

        private bool _loading;
        public bool Loading
        {
            get { return _loading; }
            set { SetProperty(ref _loading, value); }
        }

        /// <summary>
        /// 设计器使用
        /// </summary>
        public ArtistViewModel() : base(null)
        {

        }

        public ArtistViewModel(IContainerProvider containerProvider, IPlaylistService playlistService, IRegionManager regionManager) : base(containerProvider)
        {
            _playlistService = playlistService;
            _songs = new();
            _loading = false;
            _albums = new();
            _movieVideos = new();
            _regionManager = regionManager;
            _artists = new();
        }

        private DelegateCommand<Album> _openPlaylistCommand = default!;
        public DelegateCommand<Album> OpenPlaylistCommand => _openPlaylistCommand ??= new DelegateCommand<Album>((album) =>
        {
            var parameters = new NavigationParameters();
            parameters.Add("PlaylistId", album.Id);
            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(nameof(AlbumView), parameters);
        });

        private DelegateCommand<IPlaylist> _openArtistCommand = default!;
        public DelegateCommand<IPlaylist> OpenArtistCommand => _openArtistCommand ??= new DelegateCommand<IPlaylist>((playlist) =>
        {
            var parameters = new NavigationParameters();
            parameters.Add(ArtistViewModel.ArtistIdParameterName, playlist.Id);
            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(nameof(ArtistView), parameters);
        });

        public bool KeepAlive => false;

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

            ArtistId = parameters.GetValue<long>(ArtistIdParameterName);
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

        async Task GetHotSongs()
        {
            var response = await _playlistService.Artists(ArtistId);
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

        async Task GetAlbums()
        {
            var response = await _playlistService.ArtistAlbum(new Common.Models.Request.ArtistAlbumRequest
            {
                Id = ArtistId,
                Limit = 20
            });
            if (response.Code == 200)
            {
                var albums = response.HotAlbums.Select(x =>
                {
                    x.CoverImgUrl += "?param=200y200";
                    return x;
                }).ToList();
                if (albums.Count() > 0)
                {
                    Albums.AddRange(albums);
                    Album = albums[0];
                }
            }
        }

        async Task GetMovieVideos()
        {
            var response = await _playlistService.ArtistMv(new Common.Models.Request.ArtistMvRequest
            {
                Id = ArtistId,
                Limit = 20
            });
            if (response.Code == 200)
            {
                var mvs = response.Mvs.Select(x =>
                {
                    x.CoverImgUrl += "?param=464y260";
                    return x;
                }).ToList();

                if (mvs.Count() > 0)
                {
                    MovieVideos.AddRange(mvs);
                    MovieVideo = mvs[0];
                }
            }
        }

        async Task GetArtists()
        {
            var response = await _playlistService.SimiArtist(ArtistId);
            if (response.Code == 200)
            {
                Artists.AddRange(response.Artists.Take(12).Select(x =>
                {
                    x.CoverImgUrl += "?param=200y200";
                    return x;
                }));
            }
        }
    }
}
