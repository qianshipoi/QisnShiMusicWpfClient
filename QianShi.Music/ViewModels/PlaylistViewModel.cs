using QianShi.Music.Common.Models.Response;
using QianShi.Music.Data;
using QianShi.Music.Extensions;
using QianShi.Music.Models;
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
        private readonly IDataProvider<PlaylistDetailModel, long> _dataProvider;
        private readonly ISnackbarMessageQueue _snackbarMessageQueue;
        private DelegateCommand<Song?> _playCommand = default!;
        //private DelegateCommand _moreCommand = default!;
        private PlaylistDetailModel? _detail;
        private long _playlistId;

        public PlaylistViewModel(IContainerProvider containerProvider,
            IPlaylistService playlistService,
            IPlayService playService,
            IPlayStoreService playStoreService,
            IPlaylistStoreService playlistStoreService,
            IDataProvider<PlaylistDetailModel, long> dataProvider,
            ISnackbarMessageQueue snackbarMessageQueue)
            : base(containerProvider)
        {
            _playlistService = playlistService;
            _playService = playService;
            _playStoreService = playStoreService;
            _playlistStoreService = playlistStoreService;
            _dataProvider = dataProvider;
            _snackbarMessageQueue = snackbarMessageQueue;
        }

        public PlaylistDetailModel? Detail
        {
            get => _detail;
            set => SetProperty(ref _detail, value);
        }

        public DelegateCommand<Song?> PlayCommand =>
            _playCommand ??= new(Play);

        public ObservableCollection<Song> Songs { get; set; } = new();

        private DelegateCommand<string?> _toBottomCommand = default!;
        public DelegateCommand<string?> ToBottomCommand =>
            _toBottomCommand ?? (_toBottomCommand = new DelegateCommand<string?>(ExecuteToBottomCommand,(_) =>!IsBusy && Detail !=null && Detail.Songs.Count < Detail.SongsIds.Count));

        private readonly object locker = new();

        async void ExecuteToBottomCommand(string? parameter)
        {
            if (IsBusy || Detail == null || Detail.Songs.Count == Detail.SongsIds.Count) return;
            lock (locker)
            {
                if (IsBusy || Detail == null || Detail.Songs.Count == Detail.SongsIds.Count)
                {
                    return;
                }
                IsBusy = true;
            }

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

        }

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
            if (Detail == null || Detail.Id != _playlistId)
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

        private async void Play(Song? song)
        {
            if (song != null)
            {
                await _playStoreService.PlayAsync(song);
            }
            else
            {
                if (Songs.Count == 0)
                {
                    _snackbarMessageQueue.Enqueue("该歌单没有歌曲", TimeSpan.FromSeconds(5));
                    return;
                }

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