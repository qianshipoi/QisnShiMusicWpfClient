using MaterialDesignThemes.Wpf;

using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

using QianShi.Music.Common.Models;
using QianShi.Music.Common.Models.Response;
using QianShi.Music.Services;
using QianShi.Music.Views.Dialogs;

using System.Collections.ObjectModel;

namespace QianShi.Music.ViewModels
{
    public class PlaylistViewModel : NavigationViewModel
    {
        private readonly IPlaylistService _playlistService;
        private readonly IPlayStoreService _playStoreService;
        private readonly IPlayService _playService;
        private readonly IContainerProvider _containerProvider;
        private string _title = "加载中...";
        private bool _loading = false;
        private long _playlistId;
        private PlaylistDetail _detail = new();
        private ObservableCollection<Song> _playlists;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public ObservableCollection<Song> Playlists
        {
            get => _playlists;
            set => SetProperty(ref _playlists, value);
        }
        public bool Loading { get => _loading; set => SetProperty(ref _loading, value); }
        public PlaylistDetail Detail { get => _detail; set => SetProperty(ref _detail, value); }

        /// <summary>
        /// 播放歌单
        /// </summary>
        private DelegateCommand<Song?> _playCommand = default!;
        public DelegateCommand<Song?> PlayCommand =>
            _playCommand ??= new DelegateCommand<Song?>(Play);

        /// <summary>
        /// 立即播放
        /// </summary>
        public DelegateCommand<Song?> PlayImmediatelyCommand => PlayCommand;

        public PlaylistViewModel(IContainerProvider containerProvider,
            IPlaylistService playlistService, IPlayService playService, IPlayStoreService playStoreService) : base(containerProvider)
        {
            _playlists = new ObservableCollection<Song>();
            _playlistService = playlistService;
            _containerProvider = containerProvider;
            _playService = playService;
            _playStoreService = playStoreService;
        }

        private async void Play(Song? playlist)
        {
            if (playlist != null)
            {
                await _playStoreService.PlayAsync(playlist);
            }
            else
            {
                await _playStoreService.AddPlaylistAsync(_playlistId, Playlists);
            }
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            _playlistId = navigationContext.Parameters.GetValue<long>("PlaylistId");
            Title = _playlistId.ToString();
            if (Detail.Id != _playlistId)
            {
                Loading = true;
                var response = await _playlistService.GetPlaylistDetailAsync(_playlistId);
                if (response.Code == 200)
                {
                    Detail.Id = response.PlaylistDetail.Id;
                    Detail.Name = response.PlaylistDetail.Name;
                    Detail.Description = response.PlaylistDetail.Description ?? String.Empty;
                    Detail.LastUpdateTime = response.PlaylistDetail.UpdateTime;
                    Detail.PicUrl = response.PlaylistDetail.CoverImgUrl;
                    Detail.Count = response.PlaylistDetail.TrackCount;
                    Detail.Creator = response.PlaylistDetail.Creator?.Nickname;
                    _playlists.Clear();

                    // 获取所有歌曲
                    var ids = string.Join(',', response.PlaylistDetail.TrackIds.Select(x => x.Id));
                    var songResponse = await _playlistService.SongDetail(ids);
                    if (songResponse.Code == 200)
                    {
                        int i = 0;
                        foreach (var song in songResponse.Songs)
                        {
                            song.Album.CoverImgUrl += "?param=48y48";
                            _playlists.Add(song);
                            i++;
                            if (i % 5 == 0)
                            {
                                await Task.Delay(20);
                            }
                        }
                    }
                }
                Loading = false;
            }

            base.OnNavigatedTo(navigationContext);
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            if (DialogHost.IsDialogOpen(Extensions.PrismManager.PlaylistDialogName))
            {
                var session = DialogHost.GetDialogSession(Extensions.PrismManager.PlaylistDialogName);
                if (session != null)
                    session.UpdateContent(new LoadingDialog());
                DialogHost.Close(Extensions.PrismManager.PlaylistDialogName);
            }
            base.OnNavigatedFrom(navigationContext);
        }
    }
}