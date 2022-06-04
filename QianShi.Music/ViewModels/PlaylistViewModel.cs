using MaterialDesignThemes.Wpf;

using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

using QianShi.Music.Common.Models;
using QianShi.Music.Common.Models.Response;
using QianShi.Music.Services;
using QianShi.Music.Views.Dialogs;

using System.Collections.ObjectModel;

using static System.String;

namespace QianShi.Music.ViewModels
{
    public class PlaylistViewModel : NavigationViewModel
    {
        public const string IdParameters = "PlaylistId";
        private readonly IPlaylistService _playlistService;
        private readonly IPlayService _playService;
        private readonly IPlayStoreService _playStoreService;
        private PlaylistDetail _detail = new();
        private bool _loading;
        private DelegateCommand<Song?> _playCommand = default!;
        private long _playlistId;
        private ObservableCollection<Song> _songs;
        private string _title = "加载中...";

        public PlaylistViewModel(IContainerProvider containerProvider,
            IPlaylistService playlistService,
            IPlayService playService,
            IPlayStoreService playStoreService)
            : base(containerProvider)
        {
            _songs = new ObservableCollection<Song>();
            _playlistService = playlistService;
            _playService = playService;
            _playStoreService = playStoreService;
        }

        public PlaylistDetail Detail
        {
            get => _detail;
            set => SetProperty(ref _detail, value);
        }

        public bool Loading
        {
            get => _loading;
            set => SetProperty(ref _loading, value);
        }

        public DelegateCommand<Song?> PlayCommand =>
            _playCommand ??= new(Play);

        public ObservableCollection<Song> Songs
        {
            get => _songs;
            set => SetProperty(ref _songs, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
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
            _playStoreService.CurrentChanged -= CurrentChanged;
            base.OnNavigatedFrom(navigationContext);
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            _playlistId = navigationContext.Parameters.GetValue<long>(IdParameters);
            Title = _playlistId.ToString();
            if (Detail.Id != _playlistId)
            {
                Loading = true;
                var response = await _playlistService.GetPlaylistDetailAsync(_playlistId);
                if (response.Code == 200)
                {
                    Detail.Id = response.PlaylistDetail.Id;
                    Detail.Name = response.PlaylistDetail.Name;
                    Detail.Description = response.PlaylistDetail.Description ?? Empty;
                    Detail.LastUpdateTime = response.PlaylistDetail.UpdateTime;
                    Detail.PicUrl = response.PlaylistDetail.CoverImgUrl;
                    Detail.Count = response.PlaylistDetail.TrackCount;
                    Detail.Creator = response.PlaylistDetail.Creator?.Nickname;
                    Detail.CreatorId = response.PlaylistDetail.Creator?.UserId ?? 0;
                    _songs.Clear();

                    // 获取所有歌曲
                    var ids = Join(',', response.PlaylistDetail.TrackIds.Select(x => x.Id));
                    var songResponse = await _playlistService.SongDetail(ids);
                    if (songResponse.Code == 200)
                    {
                        int i = 0;
                        foreach (var song in songResponse.Songs)
                        {
                            song.Album.CoverImgUrl += "?param=48y48";
                            _songs.Add(song);
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
                    }
                }
                Loading = false;
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