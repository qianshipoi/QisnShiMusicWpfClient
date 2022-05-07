using Prism.Ioc;
using Prism.Regions;

using QianShi.Music.Common;
using QianShi.Music.Common.Models.Response;
using QianShi.Music.Services;

using System.Collections.ObjectModel;
using System.Windows;

using static QianShi.Music.Common.Models.Response.MvSublistResponse;
using static QianShi.Music.Common.Models.Response.UserCloudResponse;
using static QianShi.Music.Common.Models.Response.UserRecordResponse;

namespace QianShi.Music.ViewModels
{
    public class LibraryViewModel : NavigationViewModel
    {
        public LibraryViewModel()
            : base(App.Current.Container.Resolve<IContainerProvider>()) { }

        private readonly IPlaylistService _playlistService;

        private ObservableCollection<MovieVideoSubject> _movieVideos;
        public ObservableCollection<MovieVideoSubject> MovieVideos
        {
            get { return _movieVideos; }
            set { SetProperty(ref _movieVideos, value); }
        }

        private ObservableCollection<Album> _albums;
        public ObservableCollection<Album> Albums
        {
            get { return _albums; }
            set { SetProperty(ref _albums, value); }
        }

        private ObservableCollection<Artist> _artists;
        public ObservableCollection<Artist> Artists
        {
            get { return _artists; }
            set { SetProperty(ref _artists, value); }
        }
        private Playlist _likePlaylist;
        public Playlist LikePlaylist
        {
            get { return _likePlaylist; }
            set { SetProperty(ref _likePlaylist, value); }
        }

        private ObservableCollection<Playlist> _playlists;
        public ObservableCollection<Playlist> Playlists
        {
            get { return _playlists; }
            set { SetProperty(ref _playlists, value); }
        }

        private ObservableCollection<CloudItem> _cloudItems;
        public ObservableCollection<CloudItem> CloudItems
        {
            get { return _cloudItems; }
            set { SetProperty(ref _cloudItems, value); }
        }

        private ObservableCollection<PlayRecord> _allRecord;
        public ObservableCollection<PlayRecord> AllRecord
        {
            get { return _allRecord; }
            set { SetProperty(ref _allRecord, value); }
        }

        private ObservableCollection<PlayRecord> _weekRecord;
        public ObservableCollection<PlayRecord> WeekRecord
        {
            get { return _weekRecord; }
            set { SetProperty(ref _weekRecord, value); }
        }

        private ObservableCollection<Song> _songs;
        public ObservableCollection<Song> Songs
        {
            get { return _songs; }
            set { SetProperty(ref _songs, value); }
        }

        public LibraryViewModel(IContainerProvider containerProvider, IPlaylistService playlistService) : base(containerProvider)
        {
            _movieVideos = new();
            _playlists = new();
            _albums = new();
            _cloudItems = new();
            _allRecord = new();
            _weekRecord = new();
            _songs = new();
            _artists = new();
            _playlistService = playlistService;
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (!UserData.Instance.IsLogin)
            {
                MessageBox.Show("未登录无法使用音乐库");
                return false;
            }
            return base.IsNavigationTarget(navigationContext);
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            if (Playlists.Count == 0)
            {
                await GetMyPlaylists();

                await GetMyAlbums();
                await GetMyAritsts();
                await GetMyMvs();
                await GetMyCloud();
                await GetMyRecord();
                await GetMyRecord(1);
            }
        }

        T FormatCover<T>(T playlist) where T : IPlaylist
        {
            playlist.CoverImgUrl += "?param=200y200";
            return playlist;
        }

        async Task GetMyPlaylists()
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

        async Task GetMyLikeSongs()
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

        async Task GetMyAlbums()
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

        async Task GetMyAritsts()
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

        async Task GetMyMvs()
        {
            var response = await _playlistService.MvSublist(new Common.Models.Request.PagedRequestBase
            {
                Limit = 200,
            });
            if (response.Code == 200)
            {
                MovieVideos.AddRange(response.Data.Select(x=>
                {
                    x.CoverImgUrl += "?param=232y130";
                    return x;
                }));
            }
        }

        async Task GetMyCloud()
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

        async Task GetMyRecord(sbyte type = 0)
        {
            var allResponse = await _playlistService.UserRecord(new Common.Models.Request.UserRecordRequest
            {
                Limit = 200,
                Type = type,
                Uid = UserData.Instance.Id
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