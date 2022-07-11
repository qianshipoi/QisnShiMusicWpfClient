using QianShi.Music.Common.Models;
using QianShi.Music.Common.Models.Response;
using QianShi.Music.Data;
using QianShi.Music.Services;

namespace QianShi.Music.ViewModels
{
    public class PlaylistViewModel : NavigationViewModel
    {
        public const string IdParameters = "PlaylistId";

        private readonly IPlaylistService _playlistService;
        private readonly IPlaylistStoreService _playlistStoreService;
        private readonly IPlayService _playService;
        private readonly IPlayStoreService _playStoreService;
        private readonly IDataProvider<PlaylistDetail, long> _dataProvider;
        private DelegateCommand<Song?> _playCommand = default!;
        private DelegateCommand<ItemsControl> _moreCommand = default!;
        private PlaylistDetail _detail = default!;
        private long _playlistId;

        public PlaylistViewModel(IContainerProvider containerProvider,
            IPlaylistService playlistService,
            IPlayService playService,
            IPlayStoreService playStoreService,
            IPlaylistStoreService playlistStoreService,
            IDataProvider<PlaylistDetail, long> dataProvider)
            : base(containerProvider)
        {
            _playlistService = playlistService;
            _playService = playService;
            _playStoreService = playStoreService;
            _playlistStoreService = playlistStoreService;
            _dataProvider = dataProvider;
        }

        public PlaylistDetail Detail
        {
            get => _detail;
            set => SetProperty(ref _detail, value);
        }

        public DelegateCommand<Song?> PlayCommand =>
            _playCommand ??= new(Play);

        public ObservableCollection<Song> Songs { get; set; } = new();

        public DelegateCommand<ItemsControl> MoreCommand =>
            _moreCommand ??= new(async (control) =>
            {
                control.Focus();
                IsBusy = true;
                try
                {
                    var ids = string.Join(',', Detail.SongsIds.Skip(Detail.Songs.Count).Take(20));
                    var songsResponse = await _playlistService.SongDetail(ids);
                    if (songsResponse.Code != 200)
                    {
                        return;
                    }
                    Detail.AddSongs(songsResponse.Songs);
                    Songs.AddRange(songsResponse.Songs);
                }
                finally
                {
                    IsBusy = false;
                }
            });

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //if (DialogHost.IsDialogOpen(Extensions.PrismManager.PlaylistDialogName))
            //{
            //    var session = DialogHost.GetDialogSession(Extensions.PrismManager.PlaylistDialogName);
            //    if (session != null)
            //        session.UpdateContent(new LoadingDialog());
            //    DialogHost.Close(Extensions.PrismManager.PlaylistDialogName);
            //}
            _playStoreService.CurrentChanged -= CurrentChanged;
            base.OnNavigatedFrom(navigationContext);
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            _playlistId = navigationContext.Parameters.GetValue<long>(IdParameters);
            if (Detail.Id != _playlistId)
            {
                IsBusy = true;

                var result = await _dataProvider.GetDataAsync(_playlistId);
                if (result == null)
                {
                    navigationContext.NavigationService.Journal.GoBack();
                    return;
                }

                Detail = result;
                Songs.Clear();
                int i = 0;
                foreach (var song in Detail.Songs)
                {
                    song.Album.CoverImgUrl += "?param=48y48";
                    song.IsLike = _playlistStoreService.HasLikedSong(song);
                    Songs.Add(song);
                    i++;
                    if (i % 5 == 0)
                    {
                        await Task.Delay(20);
                    }
                }

                if ((_playStoreService.Current?.Id).HasValue)
                {
                    CurrentChanged(null, new(_playStoreService.Current));
                }

                //var response = await _playlistService.GetPlaylistDetailAsync(_playlistId);
                //if (response.Code == 200)
                //{
                //    Detail.Id = response.PlaylistDetail.Id;
                //    Detail.Name = response.PlaylistDetail.Name;
                //    Detail.Description = response.PlaylistDetail.Description ?? string.Empty;
                //    Detail.LastUpdateTime = response.PlaylistDetail.UpdateTime;
                //    Detail.PicUrl = response.PlaylistDetail.CoverImgUrl;
                //    Detail.Count = response.PlaylistDetail.TrackCount;
                //    Detail.Creator = response.PlaylistDetail.Creator?.Nickname;
                //    Detail.CreatorId = response.PlaylistDetail.Creator?.UserId ?? 0;
                //    Songs.Clear();

                //    // 获取所有歌曲
                //    if (response.PlaylistDetail.TrackIds.Count > 0)
                //    {
                //        var ids = string.Join(',', response.PlaylistDetail.TrackIds.Select(x => x.Id));
                //        var songResponse = await _playlistService.SongDetail(ids);
                //        if (songResponse.Code == 200)
                //        {
                //            int i = 0;
                //            foreach (var song in songResponse.Songs)
                //            {
                //                song.Album.CoverImgUrl += "?param=48y48";
                //                song.IsLike = _playlistStoreService.HasLikedSong(song);
                //                Songs.Add(song);
                //                i++;
                //                if (i % 5 == 0)
                //                {
                //                    await Task.Delay(20);
                //                }
                //            }

                //            if ((_playStoreService.Current?.Id).HasValue)
                //            {
                //                CurrentChanged(null, new(_playStoreService.Current));
                //            }
                //        }
                //    }
                //}
                IsBusy = false;
            }

            _playStoreService.CurrentChanged -= CurrentChanged;
            _playStoreService.CurrentChanged += CurrentChanged;

            base.OnNavigatedTo(navigationContext);
        }

        private void CurrentChanged(object? sender, SongChangedEventArgs e)
        {
            var songId = e.NewSong?.Id;
            if (songId.HasValue)
            {
                Songs.Where(x => x.IsPlaying).ToList().ForEach(item => item.IsPlaying = false);
                var song = Songs.FirstOrDefault(x => x.Id == songId.Value);
                if (song != null) song.IsPlaying = true;
            }
            else
            {
                Songs.Where(x => x.IsPlaying).ToList().ForEach(item => item.IsPlaying = false);
            }
        }

        private async void Play(Song? playlist)
        {
            if (playlist != null)
            {
                await _playStoreService.PlayAsync(playlist);
            }
            else
            {
                await _playStoreService.AddPlaylistAsync(_playlistId, Songs);
                _playStoreService.Pause();
                if (!_playService.IsPlaying)
                {
                    _playStoreService.Play();
                }
            }
        }
    }
}