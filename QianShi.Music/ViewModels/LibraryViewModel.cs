using Prism.Ioc;
using Prism.Regions;

using QianShi.Music.Common;
using QianShi.Music.Common.Models.Response;
using QianShi.Music.Services;

using System.Collections.ObjectModel;
using Prism.Commands;
using QianShi.Music.Extensions;
using QianShi.Music.Views;
using static QianShi.Music.Common.Models.Response.UserCloudResponse;

namespace QianShi.Music.ViewModels
{
    public class LibraryViewModel : NavigationViewModel
    {
        public LibraryViewModel()
            : base(App.Current.Container.Resolve<IContainerProvider>()) { }

        private readonly IPlaylistService _playlistService;
        private readonly IRegionManager _regionManager;

        private ObservableCollection<MovieVideoSubject> _movieVideos;

        public ObservableCollection<MovieVideoSubject> MovieVideos
        {
            get => _movieVideos;
            set => SetProperty(ref _movieVideos, value);
        }

        private ObservableCollection<Album> _albums;

        public ObservableCollection<Album> Albums
        {
            get => _albums;
            set => SetProperty(ref _albums, value);
        }

        private ObservableCollection<Artist> _artists;

        public ObservableCollection<Artist> Artists
        {
            get => _artists;
            set => SetProperty(ref _artists, value);
        }

        private Playlist _likePlaylist;

        public Playlist LikePlaylist
        {
            get => _likePlaylist;
            set => SetProperty(ref _likePlaylist, value);
        }

        private ObservableCollection<Playlist> _playlists;

        public ObservableCollection<Playlist> Playlists
        {
            get => _playlists;
            set => SetProperty(ref _playlists, value);
        }

        private ObservableCollection<CloudItem> _cloudItems;

        public ObservableCollection<CloudItem> CloudItems
        {
            get => _cloudItems;
            set => SetProperty(ref _cloudItems, value);
        }

        private ObservableCollection<PlayRecord> _allRecord;

        public ObservableCollection<PlayRecord> AllRecord
        {
            get => _allRecord;
            set => SetProperty(ref _allRecord, value);
        }

        private ObservableCollection<PlayRecord> _weekRecord;

        public ObservableCollection<PlayRecord> WeekRecord
        {
            get => _weekRecord;
            set => SetProperty(ref _weekRecord, value);
        }

        private ObservableCollection<Song> _songs;

        public ObservableCollection<Song> Songs
        {
            get => _songs;
            set => SetProperty(ref _songs, value);
        }

        public UserData UserInfo => UserData.Instance;

        private DelegateCommand _jumpToFondPageCommand;
        public DelegateCommand JumpToFondPageCommand =>
            _jumpToFondPageCommand ??= new(ExecuteCommandName);

        void ExecuteCommandName()
        {
            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(nameof(FondPlaylistView), new NavigationParameters
            {
                {FondPlaylistViewModel.PlaylistIdParameterName, LikePlaylist.Id}
            });
        }

        public LibraryViewModel(IContainerProvider containerProvider, IPlaylistService playlistService, IRegionManager regionManager) : base(containerProvider)
        {
            _movieVideos = new();
            _playlists = new();
            _albums = new();
            _cloudItems = new();
            _allRecord = new();
            _weekRecord = new();
            _songs = new();
            _artists = new();
            _likePlaylist = new();
            _playlistService = playlistService;
            _regionManager = regionManager;
        }

        private bool _isFirstJoin = true;

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (!UserInfo.IsLogin || UserInfo.Id == 0)
            {
                if (_isFirstJoin)
                {
                    _isFirstJoin = false;
                    navigationContext.NavigationService.Region.RequestNavigate(LoginViewModel.NavigationName);
                    return;
                }
            }

            if (Playlists.Count == 0 && UserInfo.Id != 0)
            {
                await GetMyPlaylists();
                await GetMyAlbums();
                await GetMyAritsts();
                await GetMyMvs();
                await GetMyCloud();
                await GetMyRecord(UserInfo.Id);
                await GetMyRecord(UserInfo.Id, 1);
            }
            base.OnNavigatedTo(navigationContext);
        }

        private T FormatCover<T>(T playlist) where T : IPlaylist
        {
            playlist.CoverImgUrl += "?param=200y200";
            return playlist;
        }

        private async Task GetMyPlaylists()
        {
            var myPlaylists = await _playlistService.UserPlaylist(new Common.Models.Request.UserPlaylistRequest
            {
                Limit = 200,
                Uid = UserData.Instance.Id
            });

            if (myPlaylists.Code == 200 && myPlaylists.Playlist.Count > 0)
            {
                var playlists = myPlaylists.Playlist.Select(FormatCover);

                LikePlaylist = playlists.ToList()[0];
                Playlists.AddRange(playlists.Skip(1));
                await GetMyLikeSongs();
            }
        }

        private async Task GetMyLikeSongs()
        {
            var response = await _playlistService.GetPlaylistDetailAsync(LikePlaylist.Id);
            if (response.Code == 200)
            {
                var songs = response.PlaylistDetail.Tracks.Take(12);
                Songs.AddRange(songs.Select(x =>
                {
                    if (!string.IsNullOrWhiteSpace(x.Album?.CoverImgUrl))
                        x.Album.CoverImgUrl += "?param=48y48";
                    return x;
                }));
            }
        }

        private async Task GetMyAlbums()
        {
            var response = await _playlistService.AlbumSublist(new Common.Models.Request.PagedRequestBase
            {
                Limit = 200,
            });
            if (response.Code == 200)
            {
                Albums.AddRange(response.Data.Select(FormatCover));
            }
        }

        private async Task GetMyAritsts()
        {
            var response = await _playlistService.ArtistSublist(new Common.Models.Request.PagedRequestBase
            {
                Limit = 200,
            });
            if (response.Code == 200)
            {
                Artists.AddRange(response.Data.Select(FormatCover));
            }
        }

        private async Task GetMyMvs()
        {
            var response = await _playlistService.MvSublist(new Common.Models.Request.PagedRequestBase
            {
                Limit = 200,
            });
            if (response.Code == 200)
            {
                MovieVideos.AddRange(response.Data.Select(x =>
                {
                    x.CoverImgUrl += "?param=232y130";
                    return x;
                }));
            }
        }

        private async Task GetMyCloud()
        {
            var response = await _playlistService.UserCloud(new Common.Models.Request.PagedRequestBase
            {
                Limit = 200,
            });
            if (response.Code == 200)
            {
                CloudItems.AddRange(response.Data.Select(x =>
                {
                    if (!string.IsNullOrWhiteSpace(x.SimpleSong.Album?.CoverImgUrl))
                        x.SimpleSong.Album.CoverImgUrl += "?param=48y48";
                    return x;
                }));
            }
        }

        private async Task GetMyRecord(long userId, sbyte type = 0)
        {
            var allResponse = await _playlistService.UserRecord(new Common.Models.Request.UserRecordRequest
            {
                Limit = 200,
                Type = type,
                Uid = userId
            });

            if (allResponse.Code == 200)
            {
                PlayRecord FormatCover(PlayRecord record)
                {
                    if (!string.IsNullOrWhiteSpace(record.Song.Album?.CoverImgUrl))
                        record.Song.Album = this.FormatCover(record.Song.Album);
                    return record;
                }

                if (type == 0)
                {
                    AllRecord.AddRange(allResponse.AllData.Select(FormatCover));
                }
                else
                {
                    WeekRecord.AddRange(allResponse.WeekData.Select(FormatCover));
                }
            }
        }
    }
}